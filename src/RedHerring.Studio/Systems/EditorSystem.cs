using ImGuiNET;
using NativeFileDialogSharp;
using RedHerring.Alexandria;
using RedHerring.Core;
using RedHerring.Core.Systems;
using RedHerring.Deduction;
using RedHerring.Fingerprint;
using RedHerring.Fingerprint.Layers;
using RedHerring.Fingerprint.Shortcuts;
using RedHerring.ImGui;
using RedHerring.Infusion.Attributes;
using RedHerring.Studio.Commands;
using RedHerring.Studio.Models;
using RedHerring.Studio.Models.Tests;
using RedHerring.Studio.TaskProcessing;
using RedHerring.Studio.Tools;
using RedHerring.Studio.UserInterface;
using RedHerring.Studio.UserInterface.Dialogs;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.Systems;

// TODO: Add this to engine context when creating editor window.
public sealed class EditorSystem : AnEngineSystem, IUpdatable, IDrawable
{
	[Inject] private InputSystem      _inputSystem      = null!;
	[Inject] private GraphicsSystem   _graphicsSystem   = null!;
	[Inject] private MetadataDatabase _metadataDatabase = null!;

	public bool IsEnabled   => true;
	public int  UpdateOrder => int.MaxValue;

    public bool IsVisible => true;
    public int DrawOrder => int.MaxValue;
    
    [Inject]
    private InputReceiver _inputReceiver = null!;

	private          StudioModel    _studioModel = new();
	private readonly CommandHistory _history     = new();

	private readonly List<ATool> _activeTools = new();
    
	#region User Interface
	private readonly DockSpace      _dockSpace       = new();
	private readonly Menu           _menu            = new();
	private readonly StatusBar      _statusBar       = new();
	private          SettingsDialog _projectSettings = null!;
	private          SettingsDialog _studioSettings = null!;
	private readonly MessageBox     _messageBox      = new();
	#endregion
    
	#region Lifecycle

    protected override void Init()
    {
        _inputReceiver.Name = "editor";
        _inputReceiver.ConsumesAllInput = false;
        
		_inputReceiver.Bind("undo", Undo);
		_inputReceiver.Bind("redo", Redo);
	}

	protected override void Load()
	{
		Gui.GetIO().ConfigFlags |= ImGuiConfigFlags.DockingEnable;

		InitInput();

		InitMenu();

		// debug
		_activeTools.Add(new ToolProjectView(_studioModel));
		_projectSettings = new SettingsDialog("Project settings", _history, _studioModel.ProjectSettings);
		_studioSettings = new SettingsDialog("Studio settings", _history, _studioModel.StudioSettings);
	}

	protected override void Unload()
	{
		_studioModel.Cancel(); // TODO - should we wait for cancellation of all threads?
	}

	public void Update(GameTime gameTime)
	{
		_dockSpace.Update();
        
		_menu.Update();
        
		UpdateStatusBarMessage();
		_statusBar.Update();

		_projectSettings.Update();
		_studioSettings.Update();
		_messageBox.Update();

		for(int i=0;i <_activeTools.Count;++i)
		{
			_activeTools[i].Update(out bool finished);
			if (finished)
			{
				_activeTools.RemoveAt(i);
				--i;
			}
		}

		//Gui.ShowDemoWindow();
	}

	private void UpdateStatusBarMessage()
	{
		int workerThreadsCount = _studioModel.TaskProcessor.WorkerThreadsCount;
		int remainingTasks     = _studioModel.TaskProcessor.GetRemainingTasks();
		int availableThreads   = _studioModel.TaskProcessor.AvailableWorkerThreads;
		
		if (remainingTasks > 0)
		{
			_statusBar.Message = $"Processing {remainingTasks} tasks on {workerThreadsCount} threads.";
		}
		else
		{
			_statusBar.Message = $"Ready. {availableThreads} of {workerThreadsCount} threads available.";
		}
		
		_statusBar.MessageColor = remainingTasks == 0 && availableThreads == workerThreadsCount ? StatusBar.Color.Info : StatusBar.Color.Warning;
	}
    
	public bool BeginDraw() => true;

	public void Draw(GameTime gameTime)
	{
	}

	public void EndDraw()
	{
	}

	#endregion Lifecycle

	#region Private

    private void InitInput()
    {
        _inputSystem.AddBinding(new ShortcutBinding("undo", new KeyboardShortcut(Key.U)));
        _inputSystem.AddBinding(new ShortcutBinding("redo", new KeyboardShortcut(Key.Z)));
        _inputReceiver.Push();
    }
    #endregion Private

	#region Menu
	private void InitMenu()
	{
		_menu.AddItem("File/Open project..",                      OnOpenProjectClicked);
		_menu.AddItem("File/Settings/Theme/Embrace the Darkness", Theme.EmbraceTheDarkness);
		_menu.AddItem("File/Settings/Theme/Crimson Rivers",       Theme.CrimsonRivers);
		_menu.AddItem("File/Settings/Theme/Bloodsucker",          Theme.Bloodsucker);
		_menu.AddItem("File/Exit",                                OnExitClicked);

		_menu.AddItem("Edit/Undo",               _history.Undo);
		_menu.AddItem("Edit/Redo",               _history.Redo);
		_menu.AddItem("Edit/Project settings..", OnEditProjectSettingsClicked);
		_menu.AddItem("Edit/Studio settings..", OnEditStudioSettingsClicked);

		_menu.AddItem("View/Project",   OnViewProjectClicked);
		_menu.AddItem("View/Console",   OnViewConsoleClicked);
		_menu.AddItem("View/Inspector", OnViewInspectorClicked);

		_menu.AddItem("Debug/Modal window",        () => Gui.OpenPopup("MessageBox"));
		_menu.AddItem("Debug/Task processor test", OnDebugTaskProcessorTestClicked);
		_menu.AddItem("Debug/Serialization test",  OnDebugSerializationTestClicked);
		_menu.AddItem("Debug/Importer test",       OnDebugImporterTestClicked);
	}

	private async void OnOpenProjectClicked()
	{
		DialogResult result = Dialog.FolderPicker();
		if(!result.IsOk)
		{
			return;
		}
        
		await _studioModel.OpenProject(result.Path);
	}

	private void OnExitClicked()
	{
		Context.Engine?.Exit();
	}

	private void OnEditProjectSettingsClicked()
	{
		_projectSettings.Open();
	}

	private void OnEditStudioSettingsClicked()
	{
		_studioSettings.Open();
	}
	
	private void OnViewProjectClicked()
	{
		_activeTools.Add(new ToolProjectView(_studioModel));
	}

	private void OnViewConsoleClicked()
	{
		_activeTools.Add(new ToolConsole(_studioModel));
	}

	private void OnViewInspectorClicked()
	{
		_activeTools.Add(new ToolInspector(_studioModel, _history));
	}

	private void OnDebugTaskProcessorTestClicked()
	{
		for(int i=0;i <20;++i)
		{
			_studioModel.TaskProcessor.EnqueueTask(new TestTask(i), 0);
		}
	}

	private void OnDebugSerializationTestClicked()
	{
		SerializationTests.Test();
	}

	private void OnDebugImporterTestClicked()
	{
		ImporterTests.Test();
	}
	#endregion
    
	#region Input

	private void Undo(ref ActionEvent evt)
	{
		evt.Consumed = true;
        
		_history.Undo();
	}

	private void Redo(ref ActionEvent evt)
	{
		evt.Consumed = true;
        
		_history.Redo();
	}

	#endregion Input
}
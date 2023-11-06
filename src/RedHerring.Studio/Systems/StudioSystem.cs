using ImGuiNET;
using NativeFileDialogSharp;
using RedHerring.Alexandria;
using RedHerring.Alexandria.Extensions;
using RedHerring.Core;
using RedHerring.Core.Systems;
using RedHerring.Deduction;
using RedHerring.Fingerprint;
using RedHerring.Fingerprint.Layers;
using RedHerring.Fingerprint.Shortcuts;
using RedHerring.ImGui;
using RedHerring.Infusion.Attributes;
using RedHerring.Studio.Models;
using RedHerring.Studio.Models.Tests;
using RedHerring.Studio.TaskProcessing;
using RedHerring.Studio.Tools;
using RedHerring.Studio.UserInterface;
using RedHerring.Studio.UserInterface.Dialogs;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.Systems;

public sealed class StudioSystem : AnEngineSystem, IUpdatable, IDrawable
{
	[Infuse] private InputSystem      _inputSystem      = null!;
	[Infuse] private GraphicsSystem   _graphicsSystem   = null!;
	[Infuse] private MetadataDatabase _metadataDatabase = null!;

	public bool IsEnabled   => true;
	public int  UpdateOrder => int.MaxValue;

	public bool IsVisible => true;
	public int  DrawOrder => int.MaxValue;
    
	[Infuse]
	private InputReceiver _inputReceiver = null!;

	private StudioModel _studioModel = new();

	[Infuse] private ToolManager _toolManager;
	
	#region User Interface
	private readonly DockSpace      _dockSpace       = new();
	private readonly Menu           _menu            = new();
	private readonly StatusBar      _statusBar       = new();
	private          SettingsDialog _projectSettings = null!;
	private          SettingsDialog _studioSettings  = null!;
	private readonly MessageBox     _messageBox      = new();
	#endregion
    
	#region Lifecycle

	protected override void Init()
	{
		_inputReceiver.Name             = "studio";
		_inputReceiver.ConsumesAllInput = false;
        
		_inputReceiver.Bind("undo", Undo);
		_inputReceiver.Bind("redo", Redo);
	}

	protected override async ValueTask<int> Load()
	{
		Gui.GetIO().ConfigFlags |= ImGuiConfigFlags.DockingEnable;

		// inits
		InitInput();
		InitMenu();
		_toolManager.Init(_studioModel);

		// load settings and restore state
		await LoadSettingsAsync();
		
		// debug
		_projectSettings = new SettingsDialog("Project settings", _studioModel.CommandHistory, _studioModel.ProjectSettings);
		_studioSettings  = new SettingsDialog("Studio settings",  _studioModel.CommandHistory, _studioModel.StudioSettings);
		return 0;
	}

	protected override async ValueTask<int> Unload()
	{
		await SaveSettingsAsync();
		
		_studioModel.Cancel(); // TODO - should we wait for cancellation of all threads?
		return 0;
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

		_toolManager.Update();

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
		_menu.AddItem("File/Open project..", OnOpenProjectClicked);
		_menu.AddItem("File/Exit",           OnExitClicked);

		_menu.AddItem("Edit/Undo",               _studioModel.CommandHistory.Undo);
		_menu.AddItem("Edit/Redo",               _studioModel.CommandHistory.Redo);
		_menu.AddItem("Edit/Project settings..", OnEditProjectSettingsClicked);
		_menu.AddItem("Edit/Studio settings..",  OnEditStudioSettingsClicked);

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
		_toolManager.Activate(ToolProjectView.ToolName);
	}

	private void OnViewConsoleClicked()
	{
		_toolManager.Activate(ToolConsole.ToolName);
	}

	private void OnViewInspectorClicked()
	{
		_toolManager.Activate(ToolInspector.ToolName);
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
        
		_studioModel.CommandHistory.Undo();
	}

	private void Redo(ref ActionEvent evt)
	{
		evt.Consumed = true;
        
		_studioModel.CommandHistory.Redo();
	}

	#endregion Input
	
	#region Settings
	private async Task SaveSettingsAsync()
	{
		_studioModel.StudioSettings.StoreToolWindows(ATool.UniqueToolIdGeneratorState, _toolManager.ExportActiveTools());
		
		_studioModel.StudioSettings.UiLayout = Gui.SaveIniSettingsToMemory();
		await _studioModel.SaveStudioSettingsAsync();
	}

	private async Task LoadSettingsAsync()
	{
		await _studioModel.LoadStudioSettingsAsync();

		ATool.SetUniqueIdGenerator(_studioModel.StudioSettings.ToolUniqueIdGeneratorState);
		_toolManager.ImportActiveTools(_studioModel.StudioSettings.ActiveToolWindows);
		
		if (_studioModel.StudioSettings.UiLayout != null)
		{
			Gui.LoadIniSettingsFromMemory(_studioModel.StudioSettings.UiLayout);
		}

		_studioModel.StudioSettings.ApplyTheme();
	}
	#endregion
}
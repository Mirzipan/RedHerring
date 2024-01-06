using ImGuiNET;
using NativeFileDialogSharp;
using RedHerring.Alexandria;
using RedHerring.Core;
using RedHerring.Core.Systems;
using RedHerring.Deduction;
using RedHerring.Fingerprint.Layers;
using RedHerring.Infusion.Attributes;
using RedHerring.Render.ImGui;
using RedHerring.Studio.Constants;
using RedHerring.Studio.Models;
using RedHerring.Studio.Models.Project.Importers;
using RedHerring.Studio.Models.Tests;
using RedHerring.Studio.Tools;
using RedHerring.Studio.UserInterface;
using RedHerring.Studio.UserInterface.Dialogs;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.Systems;

public sealed class StudioSystem : EngineSystem, Updatable, Drawable
{
	[Infuse] private PathsSystem      _paths            = null!;
	[Infuse] private InputSystem      _inputSystem      = null!;
	[Infuse] private InputReceiver    _inputReceiver    = null!;
	[Infuse] private GraphicsSystem   _graphicsSystem   = null!;
	[Infuse] private MetadataDatabase _metadataDatabase = null!;
	[Infuse] private ToolManager      _toolManager;
	[Infuse] private ImporterRegistry _importerRegistry = null!;
	[Infuse] private StudioCamera     _camera           = null!;

	private NewProjectDialog _newProjectDialog = null!;
	
	public bool IsEnabled   => true;
	public int  UpdateOrder => int.MaxValue;

	public bool IsVisible => true;
	public int  DrawOrder => int.MaxValue;

	private StudioModel    _studioModel = null!;
	
	#region User Interface
	private readonly DockSpace               _dockSpace       = new();
	private readonly Menu                    _menu            = new(MenuStyle.MainMenu);
	private readonly StatusBar               _statusBar       = new();
	private          ObjectDialog            _projectSettings = null!;
	private          ObjectDialog            _studioSettings  = null!;
	private readonly MessageBox              _messageBox      = new();
	private StatusBarMessageHandler _statusBarMessageHandler = null!;
	#endregion
    
	#region Lifecycle
	protected override void Init()
	{
		_studioModel = new StudioModel(_importerRegistry);
		
		_inputReceiver.Name             = "studio";
		_inputReceiver.ConsumesAllInput = false;
        
		_inputReceiver.Bind(InputAction.Undo, Undo);
		_inputReceiver.Bind(InputAction.Redo, Redo);

		_statusBarMessageHandler = new StatusBarMessageHandler(_statusBar, _studioModel);
		_newProjectDialog        = new NewProjectDialog(_studioModel);
	}

	protected override async ValueTask<int> Load()
	{
		Gui.GetIO().ConfigFlags |= ImGuiConfigFlags.DockingEnable;

		// inits
		InitInput();
		InitMenu();
		_toolManager.Init(_studioModel);

		// load settings and restore state
		LoadSettings();
		
		// debug
		_projectSettings = new ObjectDialog("Project settings", _studioModel.CommandHistory, _studioModel.Project.ProjectSettings);
		_studioSettings  = new ObjectDialog("Studio settings",  _studioModel.CommandHistory, _studioModel.StudioSettings);

		return 0;
	}

	protected override async ValueTask<int> Unload()
	{
		SaveSettings();
		
		_studioModel.Cancel(); // TODO - should we wait for cancellation of all threads?
		
		return 0;
	}

	public void Update(GameTime gameTime)
	{
		_dockSpace.Update();
        
		_menu.Update();
		_menu.InvokeClickActions();

		_statusBarMessageHandler.Update();
		_statusBar.Update();

		_projectSettings.Update();
		_studioSettings.Update();
		_messageBox.Update();
		_newProjectDialog.Update();

		_toolManager.Update();
		
		//Gui.ShowDemoWindow();
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
		_inputReceiver.Push();
	}
	#endregion Private

	#region Menu
	private void InitMenu()
	{
		_menu.AddItem("File/New project..",  OnNewProjectClicked);
		_menu.AddItem("File/Open project..", OnOpenProjectClicked);
		_menu.AddItem("File/Exit",           OnExitClicked);

		_menu.AddItem("Edit/Undo",               _studioModel.CommandHistory.Undo);
		_menu.AddItem("Edit/Redo",               _studioModel.CommandHistory.Redo);
		_menu.AddItem("Edit/Project settings..", OnEditProjectSettingsClicked);
		_menu.AddItem("Edit/Studio settings..",  OnEditStudioSettingsClicked);

		// TODO - tools should be generated from tool manager
		_menu.AddItem($"View/{FontAwesome6.FolderTree} Project",   OnViewProjectClicked);
		_menu.AddItem($"View/{FontAwesome6.Terminal} Console",   OnViewConsoleClicked);
		_menu.AddItem($"View/{FontAwesome6.CircleInfo} Inspector", OnViewInspectorClicked);

		_menu.AddItem("Debug/Modal window",        () => Gui.OpenPopup("MessageBox"));
		_menu.AddItem("Debug/Serialization test",  OnDebugSerializationTestClicked);
		_menu.AddItem("Debug/Importer test",       OnDebugImporterTestClicked);
		_menu.AddItem("Debug/Inspector test",      OnDebugInspectorTestClicked);
	}

	private void OnNewProjectClicked()
	{
		_newProjectDialog.Open();
	}

	private void OnOpenProjectClicked()
	{
		DialogResult result = Dialog.FolderPicker();
		if(!result.IsOk)
		{
			return;
		}
        
		_studioModel.OpenProject(result.Path);
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

	private void OnDebugSerializationTestClicked()
	{
		SerializationTests.Test();
	}

	private void OnDebugImporterTestClicked()
	{
		ImporterTests.Test();
	}

	private void OnDebugInspectorTestClicked()
	{
		(_toolManager.Get(ToolInspector.ToolName) as ToolInspector)?.Test();
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
	private void SaveSettings()
	{
		_studioModel.StudioSettings.StoreToolWindows(Tool.UniqueToolIdGeneratorState, _toolManager.ExportActiveTools());
		
		_studioModel.StudioSettings.UiLayout = Gui.SaveIniSettingsToMemory();
		_studioModel.SaveStudioSettings(_paths.ApplicationData);
	}

	private void LoadSettings()
	{
		_studioModel.LoadStudioSettings(_paths.ApplicationData);

		Tool.SetUniqueIdGenerator(_studioModel.StudioSettings.ToolUniqueIdGeneratorState);
		_toolManager.ImportActiveTools(_studioModel.StudioSettings.ActiveToolWindows);
		
		if (_studioModel.StudioSettings.UiLayout != null)
		{
			Gui.LoadIniSettingsFromMemory(_studioModel.StudioSettings.UiLayout);
		}

		_studioModel.StudioSettings.ApplyTheme();
	}
	#endregion
}
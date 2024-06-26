﻿using ImGuiNET;
using NativeFileDialogSharp;
using RedHerring.Alexandria;
using RedHerring.Core;
using RedHerring.Core.Systems;
using RedHerring.Deduction;
using RedHerring.Infusion.Attributes;
using RedHerring.Inputs.Layers;
using RedHerring.Studio.Constants;
using RedHerring.Studio.Models;
using RedHerring.Studio.Models.Project;
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
	[Infuse] private InputLayer    _inputLayer    = null!;
	[Infuse] private GraphicsSystem   _graphicsSystem   = null!;
	[Infuse] private StudioCamera     _camera           = null!;

	public StudioCamera Camera => _camera;
	
	private NewProjectDialog _newProjectDialog = null!;
	
	public bool IsEnabled   => true;
	public int  UpdateOrder => int.MaxValue;

	public bool IsVisible => true;
	public int  DrawOrder => int.MaxValue;

	private StudioModel    _studioModel = null!;
	private ImporterRegistry _importerRegistry = null!;
	private ToolManager _toolManager = null!;
	
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
		_importerRegistry = Findings.IndexerByType<ImporterRegistry>()!;
		_toolManager = Findings.IndexerByType<ToolManager>()!;

		_studioModel = new StudioModel(_importerRegistry);
		
		_inputLayer.Name             = "studio";
		_inputLayer.ConsumesAllInput = false;
        
		_inputLayer.Bind(InputAction.Undo, Undo);
		_inputLayer.Bind(InputAction.Redo, Redo);

		_statusBarMessageHandler = new StatusBarMessageHandler(_statusBar, _studioModel);
		_newProjectDialog        = new NewProjectDialog(_studioModel);

		_studioModel.EventAggregator.Register<OnProjectOpened>(OnProjectOpened);
		_studioModel.EventAggregator.Register<OnProjectClosed>(OnProjectClosed);
	}

	protected override async ValueTask<int> Load()
	{
		Gui.GetIO().ConfigFlags |= ImGuiConfigFlags.DockingEnable;

		// inits
		InitInput();
		InitMenu();
		_toolManager.Init(_studioModel, this);

		// load settings and restore state
		LoadSettings();
		
		// dialogs
		CreateProjectSettings();
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

	public void FocusChanged(bool hasFocus)
	{
		if (hasFocus)
		{
			_studioModel.Project.ResumeWatchers();
		}
		else
		{
			_studioModel.Project.PauseWatchers();
		}
	}
	#endregion Lifecycle

	#region Private

	private void InitInput()
	{
		_inputLayer.Push();
	}

	private void CreateProjectSettings()
	{
		_projectSettings = new ObjectDialog("Project settings", _studioModel.CommandHistory, _studioModel.Project.ProjectSettings);
	}

	#endregion Private

	#region Event handlers
	private void OnProjectOpened(OnProjectOpened obj)
	{
		Context.Window.Title = $"{Program.Title} - {_studioModel.Project.ProjectSettings.ProjectFolderPath}";
	}

	private void OnProjectClosed(OnProjectClosed obj)
	{
		Context.Window.Title = Program.Title;
	}
	#endregion
	
	#region Menu
	private void InitMenu()
	{
		_menu.AddItem("File/New project..",  OnNewProjectClicked);
		_menu.AddItem("File/Open project..", OnOpenProjectClicked);
		_menu.AddItem("File/Exit",           OnExitClicked);

		_menu.AddItem("Edit/Undo",               _studioModel.CommandHistory.Undo);
		_menu.AddItem("Edit/Redo",               _studioModel.CommandHistory.Redo);
		_menu.AddItem("Edit/Project settings..", OnEditProjectSettingsClicked, () => _studioModel.Project.IsOpened);
		_menu.AddItem("Edit/Studio settings..",  OnEditStudioSettingsClicked);

		// TODO - tools should be generated from tool manager
		_menu.AddItem($"View/{ToolProjectView.ToolName}",   OnViewProjectClicked);
		_menu.AddItem($"View/{ToolConsole.ToolName}",     OnViewConsoleClicked);
		_menu.AddItem($"View/{ToolInspector.ToolName}", OnViewInspectorClicked);
		_menu.AddItem($"View/{ToolFilePreview.ToolName}", OnViewTextEditorClicked);
		_menu.AddItem($"View/{ToolDebug.ToolName}", OnViewDebugClicked);

		_menu.AddItem("Project/Update engine files", OnProjectUpdateEngineFilesClicked, () => _studioModel.Project.IsOpened);
		_menu.AddItem("Project/Clear Resources",     OnProjectClearResourcesClicked,    () => _studioModel.Project.IsOpened);
		_menu.AddItem("Project/Reimport all",        OnProjectReimportAllClicked,       () => _studioModel.Project.IsOpened);
		_menu.AddItem("Project/Import changed",      OnProjectImportChangedClicked,     () => _studioModel.Project.IsOpened);
		
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
		CreateProjectSettings();
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

	private void OnViewTextEditorClicked()
	{
		_toolManager.Activate(ToolFilePreview.ToolName);
	}

	private void OnViewDebugClicked()
	{
		_toolManager.Activate(ToolDebug.ToolName);
	}

	private void OnProjectUpdateEngineFilesClicked()
	{
		_studioModel.Project.UpdateEngineFiles();
	}

	private void OnProjectClearResourcesClicked()
	{
		_studioModel.Project.ClearResources();
	}

	private void OnProjectReimportAllClicked()
	{
		_studioModel.Project.ClearResources();
		_studioModel.Project.ImportAll(true);
	}

	private void OnProjectImportChangedClicked()
	{
		_studioModel.Project.ImportAll(false);
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
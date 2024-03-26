using ImGuiNET;
using NativeFileDialogSharp;
using RedHerring.Studio.Commands;
using RedHerring.Studio.Models;
using RedHerring.Studio.Models.ViewModels.Console;
using RedHerring.Studio.UserInterface.Attributes;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface.Dialogs;

public sealed class NewProjectDialog
{
	private readonly StudioModel  _studioModel;
	private readonly ObjectDialog _dialog;
	
	[ShowInInspector, OnCommitValue(nameof(UpdateTargetPath))] private string _name              = "";
	[ShowInInspector]                                          private string _path              = "";
	[ShowInInspector, ReadOnlyInInspector]                     private string _targetPath        = "";
	[ShowInInspector]                                          private bool   _openAfterCreation = true;

	public NewProjectDialog(StudioModel studioModel)
	{
		_studioModel = studioModel;
		_dialog      = new ObjectDialog("Create new project", new CommandHistory(), this);
	}
	
	public void Open()
	{
		_dialog.Open();
	}

	public void Update()
	{
		_dialog.Update();
	}

	[Button("Change path..")]
	private void Browse()
	{
		DialogResult result = Dialog.FolderPicker();
		if(!result.IsOk)
		{
			return;
		}

		_path = result.Path;
		UpdateTargetPath();
	}

	[Button("Create!")]
	private void Create()
	{
		ConsoleViewModel.Log($"Creating new project at {_targetPath}", ConsoleItemType.Info);
		try
		{
			TemplateUtility.InstantiateTemplate(_targetPath, _name);
		}
		catch(Exception e)
		{
			ConsoleViewModel.Log(e.Message,    ConsoleItemType.Exception);
			ConsoleViewModel.Log(e.StackTrace, ConsoleItemType.Exception);
			return;
		}

		// TODO - open created project
		
		Gui.CloseCurrentPopup();

		if (_openAfterCreation)
		{
			_studioModel.OpenProject(_targetPath);
		}
	}

	private void UpdateTargetPath()
	{
		_targetPath = Path.Combine(_path, _name);
	}
}
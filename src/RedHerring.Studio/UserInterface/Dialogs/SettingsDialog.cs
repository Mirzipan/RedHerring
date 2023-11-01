using System.Numerics;
using ImGuiNET;
using RedHerring.Studio.Commands;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface.Dialogs;

public sealed class SettingsDialog
{
	private bool _isOpen = true;

	private readonly string    _titleId;
	private readonly Inspector _inspector;

	public SettingsDialog(string titleId, CommandHistory history, object sourceModel)
	{
		_titleId   = titleId;
		_inspector = new Inspector(history);
		_inspector.Init(sourceModel);
	}

	public void Open()
	{
		Gui.OpenPopup(_titleId);
	}

	public void Update()
	{
		Vector2 center = Gui.GetMainViewport().GetCenter();
		Gui.SetNextWindowPos(center, ImGuiCond.Appearing, new Vector2(0.5f, 0.5f));
		Gui.SetNextWindowSizeConstraints(new Vector2(400, 200), new Vector2(2000, 2000));

		if (Gui.BeginPopupModal(_titleId, ref _isOpen))
		{
			_inspector.Update();
			Gui.EndPopup();
		}
	}
}
using System.Numerics;
using ImGuiNET;
using RedHerring.Studio.Commands;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface.Dialogs;

public sealed class ObjectDialog
{
	private readonly string    _titleId;
	private readonly Inspector _inspector;

	public ObjectDialog(string titleId, CommandHistory history, object sourceModel)
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

		bool isOpenUnused = true;
		if (Gui.BeginPopupModal(_titleId, ref isOpenUnused))
		{
			_inspector.Update();
			Gui.EndPopup();
		}
	}
}
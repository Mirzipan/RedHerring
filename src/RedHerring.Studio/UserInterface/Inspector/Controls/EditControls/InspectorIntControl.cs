using ImGuiNET;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface;

public sealed class InspectorIntControl : InspectorSingleInputControl<int>
{
	public InspectorIntControl(IInspectorCommandTarget commandTarget, string id) : base(commandTarget, id)
	{
	}

	protected override bool InputControl(bool makeItemActive)
	{
		if (makeItemActive)
		{
			Gui.SetKeyboardFocusHere();
		}

		Gui.InputInt(LabelId, ref Value, 0, 0, _isReadOnly ? ImGuiInputTextFlags.ReadOnly : ImGuiInputTextFlags.None);
		return Gui.IsItemDeactivatedAfterEdit();
	}
}
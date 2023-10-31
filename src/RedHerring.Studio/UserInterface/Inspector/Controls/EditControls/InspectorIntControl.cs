using ImGuiNET;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface;

public sealed class InspectorIntControl : AnInspectorSingleInputControl<int>
{
	public InspectorIntControl(Inspector inspector, string id) : base(inspector, id)
	{
	}

	protected override bool InputControl()
	{
		Gui.InputInt(Id, ref Value, 0, 0, _isReadOnly ? ImGuiInputTextFlags.ReadOnly : ImGuiInputTextFlags.None);
		return Gui.IsItemDeactivatedAfterEdit();
	}
}
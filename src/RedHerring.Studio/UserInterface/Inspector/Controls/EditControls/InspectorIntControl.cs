using ImGuiNET;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface;

public sealed class InspectorIntControl : AnInspectorSimpleEditControl<int>
{
	public InspectorIntControl(Inspector inspector, string id) : base(inspector, id)
	{
	}

	protected override void GuiInputControl()
	{
		int localValue = Value;
		Gui.InputInt(Id, ref localValue, 0, 0, _isReadOnly ? ImGuiInputTextFlags.ReadOnly : ImGuiInputTextFlags.None);
		Value = localValue;
	}
}
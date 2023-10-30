using ImGuiNET;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface;

public sealed class InspectorFloatControl : AnInspectorSimpleEditControl<float>
{
	public InspectorFloatControl(Inspector inspector, string id) : base(inspector, id)
	{
	}

	protected override void GuiInputControl()
	{
		float localValue = Value;
		Gui.InputFloat(Id, ref localValue, 0.0f, 0.0f, "%.3f", _isReadOnly ? ImGuiInputTextFlags.ReadOnly : ImGuiInputTextFlags.None);
		Value = localValue;
	}
}
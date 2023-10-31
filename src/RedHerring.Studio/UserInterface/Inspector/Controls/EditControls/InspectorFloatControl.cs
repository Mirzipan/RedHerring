using ImGuiNET;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface;

public sealed class InspectorFloatControl : AnInspectorSingleInputControl<float>
{
	public InspectorFloatControl(Inspector inspector, string id) : base(inspector, id)
	{
	}

	protected override bool InputControl()
	{
		Gui.InputFloat(Id, ref Value, 0.0f, 0.0f, "%.3f", _isReadOnly ? ImGuiInputTextFlags.ReadOnly : ImGuiInputTextFlags.None);
		return Gui.IsItemDeactivatedAfterEdit();
	}
}
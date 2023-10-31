using ImGuiNET;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface;

public class InspectorBoolControl : AnInspectorSingleInputControl<bool>
{
	public InspectorBoolControl(Inspector inspector, string id) : base(inspector, id)
	{
	}

	protected override void InputControl()
	{
		Gui.BeginDisabled(_isReadOnly);
		
		Gui.Checkbox(Id, ref Value);

		Gui.EndDisabled();
	}
}
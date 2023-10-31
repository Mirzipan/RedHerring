using ImGuiNET;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface;

public class InspectorBoolControl : AnInspectorSingleInputControl<bool>
{
	public InspectorBoolControl(Inspector inspector, string id) : base(inspector, id)
	{
	}

	protected override void GuiInputControl()
	{
		Gui.BeginDisabled(_isReadOnly);
		
		bool localValue = Value;
		Gui.Checkbox(Id, ref localValue);
		Value = localValue;

		Gui.EndDisabled();
	}
}
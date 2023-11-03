using ImGuiNET;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface;

public class InspectorBoolControl : AnInspectorSingleInputControl<bool>
{
	public InspectorBoolControl(Inspector inspector, string id) : base(inspector, id)
	{
	}

	protected override bool InputControl(bool makeItemActive)
	{
		Gui.BeginDisabled(_isReadOnly);
		bool submit = Gui.Checkbox(LabelId, ref Value);
		Gui.EndDisabled();

		return submit || makeItemActive;
	}
}
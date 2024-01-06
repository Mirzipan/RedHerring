using ImGuiNET;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface;

public class InspectorBoolControl : InspectorSingleInputControl<bool>
{
	public InspectorBoolControl(IInspectorCommandTarget commandTarget, string id) : base(commandTarget, id)
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
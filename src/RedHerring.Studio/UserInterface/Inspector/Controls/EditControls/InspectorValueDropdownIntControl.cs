using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface;

public class InspectorValueDropdownIntControl : InspectorValueDropdownControl<int>
{
	public InspectorValueDropdownIntControl(Inspector inspector, string id) : base(inspector, id)
	{
	}

	protected override bool InputControl(bool makeItemActive)
	{
		bool submit     = Gui.Combo(LabelId, ref Value, _items, _items.Length);
		return submit || makeItemActive;
	}
}
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface;

public class InspectorValueDropdownIntControl : InspectorValueDropdownControl<int>
{
	public InspectorValueDropdownIntControl(IInspectorCommandTarget commandTarget, string id) : base(commandTarget, id)
	{
	}

	protected override bool InputControl(bool makeItemActive)
	{
		bool submit = Gui.Combo(LabelId, ref Value, _items, _items.Length);
		return submit || makeItemActive;
	}
}
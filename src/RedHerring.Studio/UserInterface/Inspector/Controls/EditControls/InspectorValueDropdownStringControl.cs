using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface;

public class InspectorValueDropdownStringControl : InspectorValueDropdownControl<string>
{
	public InspectorValueDropdownStringControl(IInspectorCommandTarget commandTarget, string id) : base(commandTarget, id)
	{
	}

	protected override bool InputControl(bool makeItemActive)
	{
		int  localValue = Array.FindIndex(_items, x => x == Value);
		bool submit     = Gui.Combo(LabelId, ref localValue, _items, _items.Length);

		if (localValue != -1)
		{
			Value = _items[localValue];
		}

		return submit || makeItemActive;
	}
}
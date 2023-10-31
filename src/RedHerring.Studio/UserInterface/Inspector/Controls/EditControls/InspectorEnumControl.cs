using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface;

public class InspectorEnumControl : AnInspectorSingleInputControl<Enum>
{
	private readonly string[] _items;
	
	public InspectorEnumControl(Inspector inspector, string id) : base(inspector, id)
	{
		_items = Enum.GetNames(Value!.GetType());
	}

	protected override void InputControl()
	{
		int localValue = Convert.ToInt32(Value!);
		Gui.Combo(Id, ref localValue, _items, _items.Length);
		//Value = localValue;
	}
}
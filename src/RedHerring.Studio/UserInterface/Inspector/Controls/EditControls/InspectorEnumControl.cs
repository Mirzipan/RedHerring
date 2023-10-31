using System.Reflection;
using RedHerring.Studio.Tools;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface;

public class InspectorEnumControl : AnInspectorSingleInputControl<Enum>
{
	private string[] _items = null!;
	
	public InspectorEnumControl(Inspector inspector, string id) : base(inspector, id)
	{
	}

	public override void InitFromSource(object source, FieldInfo? sourceField = null)
	{
		base.InitFromSource(source, sourceField);
		_items = Enum.GetNames(Value!.GetType());
	}
	
	protected override bool InputControl()
	{
		int localValue = Convert.ToInt32(Value!);
		bool submit = Gui.Combo(Id, ref localValue, _items, _items.Length);
		Value = (Enum?)Enum.ToObject(Value!.GetType(), localValue);
		
		return submit;
	}
}
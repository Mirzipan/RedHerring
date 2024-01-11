using System.Reflection;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface;

public class InspectorEnumControl : InspectorSingleInputControl<Enum>
{
	private string[]            _items       = null!;
	private Dictionary<int,int> _enumToIndex = null!;
	private int[]               _indexToEnum = null!;
	
	public InspectorEnumControl(IInspectorCommandTarget commandTarget, string id) : base(commandTarget, id)
	{
	}

	public override void InitFromSource(object? sourceOwner, object source, FieldInfo? sourceField = null, int sourceIndex = -1)
	{
		base.InitFromSource(sourceOwner, source, sourceField, sourceIndex);
		_items = Enum.GetNames(Value!.GetType());

		_enumToIndex = new Dictionary<int,int>();
		_indexToEnum = new int[_items.Length];

		int index = 0;
		foreach (object? value in Enum.GetValues(Value!.GetType()))
		{
			int enumValue = (int) value;
			_enumToIndex[enumValue] = index;
			_indexToEnum[index]     = enumValue;
			++index;
		}
	}
	
	protected override bool InputControl(bool makeItemActive)
	{
		int  localValue = _enumToIndex[Convert.ToInt32(Value!)];
		bool submit     = Gui.Combo(LabelId, ref localValue, _items, _items.Length);
		Value = (Enum?)Enum.ToObject(Value!.GetType(), _indexToEnum[localValue]);
		return submit || makeItemActive;
	}
}
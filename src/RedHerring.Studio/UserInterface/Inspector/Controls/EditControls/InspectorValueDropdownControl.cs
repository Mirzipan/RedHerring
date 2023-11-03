using System.Reflection;
using RedHerring.Studio.UserInterface.Attributes;

namespace RedHerring.Studio.UserInterface;

// TODO: it's dangerous to use this control in two different classes! Some kind of check is needed.
public abstract class InspectorValueDropdownControl<T> : AnInspectorSingleInputControl<T>
{
	protected string[] _items = null!;

	protected InspectorValueDropdownControl(Inspector inspector, string id) : base(inspector, id)
	{
	}

	public override void InitFromSource(object? sourceOwner, object source, FieldInfo? sourceField = null)
	{
		base.InitFromSource(sourceOwner, source, sourceField);

		if (sourceOwner == null)
		{
			_items = Array.Empty<string>();
			return;
		}

		ValueDropdownAttribute dropdownAttribute = sourceField!.GetCustomAttribute<ValueDropdownAttribute>()!;
		Type                   sourceOwnerType   = sourceOwner.GetType();

		FieldInfo? itemsSourceField = sourceOwnerType.GetField(dropdownAttribute.ItemsSource, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
		if (itemsSourceField == null)
		{
			_items = Array.Empty<string>();
			return;
		}

		// obtain values from source field by field type
		if (itemsSourceField.FieldType.IsArray)
		{
			object[]? array = (object[]?) itemsSourceField.GetValue(sourceOwner);
			if (array == null)
			{
				_items = Array.Empty<string>();
				return;
			}

			_items = array.Select(x => x.ToString()!).ToArray();
			return;
		}

		_items = Array.Empty<string>();
	}
}
using System.Collections;
using System.Reflection;
using RedHerring.Studio.UserInterface.Attributes;

namespace RedHerring.Studio.UserInterface;

// TODO: it's dangerous to use this control in two different classes! Some kind of check is needed.
public abstract class InspectorValueDropdownControl<T> : InspectorSingleInputControl<T>
{
	protected string[] _items = null!;

	protected InspectorValueDropdownControl(IInspectorCommandTarget commandTarget, string id) : base(commandTarget, id)
	{
	}

	public override void InitFromSource(object? sourceOwner, object source, FieldInfo? sourceField = null, int sourceIndex = -1)
	{
		base.InitFromSource(sourceOwner, source, sourceField, sourceIndex);

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

		// obtain values from any list type
		{
			object? itemsSource = itemsSourceField.GetValue(sourceOwner);
			if (itemsSource is IList itemsSourceList)
			{
				if (itemsSourceList.Count == 0)
				{
					_items = Array.Empty<string>();
					return;
				}

				_items = new string[itemsSourceList.Count];
				for(int i=0;i<itemsSourceList.Count;++i)
				{
					_items[i] = itemsSourceList[i]?.ToString() ?? "null";
				}
				return;
			}
		}

		_items = Array.Empty<string>();
	}
}
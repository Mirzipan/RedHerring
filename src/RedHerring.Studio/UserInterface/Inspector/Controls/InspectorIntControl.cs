using System.Reflection;
using ImGuiNET;
using RedHerring.Studio.UserInterface.Attributes;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface;

public sealed class InspectorIntControl : AnInspectorControl
{
	private          int     _value          = 0;
	private          bool    _multipleValues = false;
	private readonly string? _multipleValuesLabel;
	private          bool    _isReadOnly = false;
	
	public InspectorIntControl(string id) : base(id)
	{
		_multipleValuesLabel = "Multiple values" + Id;
	}

	public override void InitFromSource(object source, FieldInfo? sourceField = null)
	{
		base.InitFromSource(source, sourceField);

		if (sourceField == null)
		{
			return;
		}
		
		_value          = sourceField.GetValue(source) as int? ?? 0;
		_multipleValues = false;
		_isReadOnly = sourceField.IsInitOnly || sourceField.GetCustomAttribute<ReadOnlyInInspectorAttribute>() != null;
	}

	public override void AdaptToSource(object source, FieldInfo? sourceField = null)
	{
		if (sourceField == null)
		{
			return;
		}

		base.AdaptToSource(source, sourceField);
		_multipleValues = GetValue() == MultipleValues;
		_isReadOnly     = _isReadOnly || ValueBindings.Any(x => x.SourceField!.IsInitOnly || x.SourceField!.GetCustomAttribute<ReadOnlyInInspectorAttribute>() != null);
	}

	public override void Update()
	{
		Gui.TextUnformatted(Label);
		Gui.SameLine();

		if(_multipleValues)
		{
			if (Gui.Button(_multipleValuesLabel))
			{
				_multipleValues = _isReadOnly; // if any of controls is read only, values cannot be edited!
			}
			return;
		}

		ImGuiInputTextFlags flags = _isReadOnly ? ImGuiInputTextFlags.ReadOnly : ImGuiInputTextFlags.EnterReturnsTrue;
		if (Gui.InputInt(Id, ref _value, 0, 0, flags))
		{
			Console.WriteLine($"Value changed to {_value}");
			SetValue(_value);
		}
	}
}
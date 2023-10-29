using System.Reflection;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface;

public sealed class InspectorIntControl : AnInspectorControl
{
	private int  _value          = 0;
	private bool _multipleValues = false;
	
	public InspectorIntControl(string id) : base(id)
	{
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
	}

	public override void AdaptToSource(object    source, FieldInfo? sourceField = null)
	{
		base.AdaptToSource(source, sourceField);
		_multipleValues = GetValue() == MultipleValues;
	}

	public override void Update()
	{
		Gui.TextUnformatted(Label);
		Gui.SameLine();

		if(_multipleValues)
		{
			if (Gui.Button("Multiple values"))
			{
				_multipleValues = false; // user clicked the button and overriden all values to the same from first binding
			}
			return;
		}

		if (Gui.InputInt(Id, ref _value))
		{
			SetValue(_value);
		}
	}
}
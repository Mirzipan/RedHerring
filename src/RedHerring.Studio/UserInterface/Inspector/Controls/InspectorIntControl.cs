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
	private          bool    _wasActive  = false;
	
	public InspectorIntControl(Inspector inspector, string id) : base(inspector, id)
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
		_isReadOnly     = sourceField.IsInitOnly || sourceField.GetCustomAttribute<ReadOnlyInInspectorAttribute>() != null;
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
		Gui.AlignTextToFramePadding();
		Gui.TextUnformatted(Label);
		Gui.SameLine();

		bool buttonClicked = false;
		if(_multipleValues)
		{
			if (_isReadOnly)
			{
				Gui.BeginDisabled();
			}
			buttonClicked = Gui.Button(_multipleValuesLabel);
			if (_isReadOnly)
			{
				Gui.EndDisabled();
			}

			if (buttonClicked)
			{
				_multipleValues = _isReadOnly; // if any of controls is read only, values cannot be edited!
				Gui.SetKeyboardFocusHere(0);   // focus next control = input
			}
			else
			{
				return;
			}
		}

		if (_isReadOnly)
		{
			Gui.PushStyleVar(ImGuiStyleVar.Alpha, 0.5f);
		}

		ImGuiInputTextFlags flags = _isReadOnly ? ImGuiInputTextFlags.ReadOnly : ImGuiInputTextFlags.None;
		Gui.InputInt(Id, ref _value, 0, 0, flags);
		bool isActive = buttonClicked || Gui.IsItemActive();

		if (_isReadOnly)
		{
			Gui.PopStyleVar();
			UpdateValue();
			return; // don't even think about submitting value
		}

		// value submission
		bool inputSubmitted = false;
		
		if(isActive && (Gui.IsKeyPressed(ImGuiKey.Enter) || Gui.IsKeyPressed(ImGuiKey.KeypadEnter)))
		{
			inputSubmitted = true; // submit on enter
		}
		else if(!isActive && _wasActive)
		{
			inputSubmitted = true; // submit on focus lost
		}
		_wasActive = isActive;
		
		if (inputSubmitted)
		{
			SubmitValue();
		}
		else if (!isActive)
		{
			UpdateValue();
		}
	}

	private void UpdateValue()
	{
		if (_multipleValues)
		{
			return;
		}

		object? value = GetValue();
		if (value == MultipleValues)
		{
			_multipleValues = true;
			return;
		}

		if (value == UnboundValue)
		{
			return;
		}

		_value = (int)(value!);
	}

	private void SubmitValue()
	{
		Console.WriteLine($"Submitted value {_value}");
		SetValue(_value);
	}
}
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
		Gui.TextUnformatted(Label);
		Gui.SameLine();

		if(_multipleValues)
		{
			if (_isReadOnly)
			{
				Gui.BeginDisabled();
			}
			bool buttonClicked = Gui.Button(_multipleValuesLabel);
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

		if (_isReadOnly)
		{
			Gui.PopStyleVar();
			return; // don't even think about submitting value
		}

		// value submission
		bool inputSubmitted = false;
		
		bool isActive = Gui.IsItemActive();
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
			Console.WriteLine($"Submitted value {_value}");
			SetValue(_value);
		}
	}
}
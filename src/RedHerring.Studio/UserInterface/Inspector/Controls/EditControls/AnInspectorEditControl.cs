﻿using System.Reflection;
using ImGuiNET;
using RedHerring.Studio.UserInterface.Attributes;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface;

public abstract class AnInspectorEditControl<T> : AnInspectorControl
{
	private const string MultipleValuesButtonLabel = "Edit multiple";
	
	private object? _value;
	protected T? Value
	{
		get => _value is T tValue ? tValue : default;
		set => _value = value;
	}
	
	protected        bool    _multipleValues = false;
	private readonly string? _multipleValuesLabel;
	
	protected bool _isReadOnly = false;
	
	protected AnInspectorEditControl(Inspector inspector, string id) : base(inspector, id)
	{
		_multipleValuesLabel = MultipleValuesButtonLabel + Id;
	}

	public override void InitFromSource(object source, FieldInfo? sourceField = null)
	{
		base.InitFromSource(source, sourceField);

		_multipleValues = false;
		_isReadOnly     = sourceField != null && (sourceField.IsInitOnly || sourceField.GetCustomAttribute<ReadOnlyInInspectorAttribute>() != null);
		_value          = sourceField?.GetValue(source);
	}

	public override void AdaptToSource(object source, FieldInfo? sourceField = null)
	{
		base.AdaptToSource(source, sourceField);

		_multipleValues = GetValue() == MultipleValues;
		_isReadOnly     = _isReadOnly || ValueBindings.Any(x => x.SourceField != null && (x.SourceField.IsInitOnly || x.SourceField.GetCustomAttribute<ReadOnlyInInspectorAttribute>() != null));
	}
	
	#region Value manipulation
	private void UpdateValue()
	{
		if (_multipleValues)
		{
			return; // no value to update (note: even in this case we could try to update value and check if it's still multiple values)
		}

		object? value = GetValue();
		if (value == MultipleValues)
		{
			_multipleValues = true; // something changed and there are multiple values now
			return;
		}

		if (value == UnboundValue)
		{
			return; // no value to update
		}

		_value = value;
	}

	protected void SubmitOrUpdateValue(bool submitted, bool controlIsActive)
	{
		if (submitted)
		{
			SetValue(_value);
			return;
		}

		if(_isReadOnly || !controlIsActive)
		{
			UpdateValue();
		}
	}
	#endregion
	
	#region Gui related
	protected bool GuiMultiEditButton()
	{
		Gui.BeginDisabled(_isReadOnly);
		bool buttonClicked = Gui.Button(_multipleValuesLabel);
		Gui.EndDisabled();
		return buttonClicked;
	}

	protected void GuiBeginReadOnlyStyle()
	{
		if (_isReadOnly)
		{
			Gui.PushStyleVar(ImGuiStyleVar.Alpha, 0.5f); // workaround to readonly input style
		}
	}

	protected void GuiEndReadOnlyStyle()
	{
		if (_isReadOnly)
		{
			Gui.PushStyleVar(ImGuiStyleVar.Alpha, 1.0f); // workaround to readonly input style
		}
	}
	#endregion
}
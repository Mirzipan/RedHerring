using System.Reflection;
using ImGuiNET;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface;

public abstract class InspectorEditControl<T> : InspectorControl
{
	private const string MultipleValuesButtonLabel = "Edit multiple";

	protected T? Value;
	
	protected        bool    _multipleValues = false;
	private readonly string? _multipleValuesLabelId;
	
	protected bool _isReadOnly = false;
	
	protected InspectorEditControl(IInspectorCommandTarget commandTarget, string id) : base(commandTarget, id)
	{
		_multipleValuesLabelId = $"{MultipleValuesButtonLabel}##{Id}";
	}

	public override void InitFromSource(object? sourceOwner, object source, FieldInfo? sourceField = null, int sourceIndex = -1)
	{
		base.InitFromSource(sourceOwner, source, sourceField, sourceIndex);

		_multipleValues =  false;
		_isReadOnly     |= Bindings.Any(x => x.IsUnbound || x.IsReadOnly);
		UpdateValue();
	}

	public override void AdaptToSource(object? sourceOwner, object source, FieldInfo? sourceField = null)
	{
		base.AdaptToSource(sourceOwner, source, sourceField);

		_multipleValues = GetValue() == MultipleValues;
		_isReadOnly     |= Bindings.Any(x => x.IsUnbound || x.IsReadOnly);
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

		Value = (T?)value;
	}

	protected void SubmitOrUpdateValue(bool submitted, bool controlIsActive)
	{
		if (!_isReadOnly && submitted)
		{
			SetValue(Value);
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
		bool buttonClicked = Gui.Button(_multipleValuesLabelId);
		Gui.EndDisabled();
		Gui.SameLine();
		Gui.AlignTextToFramePadding();
		Gui.Text(Label);
		return buttonClicked;
	}

	protected void BeginReadOnlyStyle()
	{
		if (_isReadOnly)
		{
			Gui.PushStyleVar(ImGuiStyleVar.Alpha, Gui.GetStyle().DisabledAlpha); // workaround to readonly input style
		}
	}

	protected void EndReadOnlyStyle()
	{
		if (_isReadOnly)
		{
			Gui.PopStyleVar();
		}
	}
	#endregion
}
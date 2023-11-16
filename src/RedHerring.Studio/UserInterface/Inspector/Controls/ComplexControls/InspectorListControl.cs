using System.Collections;
using ImGuiNET;
using Gui = ImGuiNET.ImGui;

using System.Reflection;
using RedHerring.Studio.UserInterface.Attributes;

namespace RedHerring.Studio.UserInterface;

/*
	Notes:
	 - there are 3 cases:
		- list of classes
			- this may use InspectorClassControl
		- list of primitive types (int, float, bool, etc..)
			- custom per this controls - cannot bind already done controls properly
		- list of references
			- custom, needs research
 */

public sealed class InspectorListControl : AnInspectorControl
{
	private object?                   _sourceOwner = null;
	private bool                      _isReadOnly = false;
	private List<AnInspectorControl?> _controls   = new(); // control per item

	public InspectorListControl(Inspector inspector, string id) : base(inspector, id)
	{
		
	}

	public override void InitFromSource(object? sourceOwner, object source, FieldInfo? sourceField = null, int sourceIndex = -1)
	{
		base.InitFromSource(sourceOwner, source, sourceField, sourceIndex);
		_sourceOwner = sourceOwner;
		_isReadOnly  = sourceField != null && (sourceField.IsInitOnly || sourceField.GetCustomAttribute<ReadOnlyInInspectorAttribute>() != null);
	}

	public override void AdaptToSource(object? sourceOwner, object source, FieldInfo? sourceField = null)
	{
		base.AdaptToSource(sourceOwner, source, sourceField);
		_isReadOnly |= sourceField != null && (sourceField.IsInitOnly || sourceField.GetCustomAttribute<ReadOnlyInInspectorAttribute>() != null);
	}
	
	public override void Update()
	{
		InspectorValueBinding valueBinding = ValueBindings.First();
		
		object? value = valueBinding.SourceField?.GetValue(valueBinding.Source);
		if (value == null)
		{
			return;
		}

		if (value is Array array) // array first, because it's a special case of IList
		{
			Type arrayElementType = valueBinding.SourceField!.FieldType.GetElementType()!;
			
			if (Gui.CollapsingHeader(LabelId, ImGuiTreeNodeFlags.DefaultOpen))
			{
				Gui.Indent();

				//foreach (AnInspectorControl control in _controls)
				for (int i = 0; i < array.Length; ++i)
				{
				}

				Gui.Unindent();
			}

			return;
		}
		
		if (value is IList list)
		{
			Type[] listElementType = valueBinding.SourceField!.FieldType.GenericTypeArguments;

			if (Gui.CollapsingHeader(LabelId, ImGuiTreeNodeFlags.DefaultOpen))
			{
				Gui.Indent();
			
				//foreach (AnInspectorControl control in _controls)
				for(int i = 0; i < list.Count; ++i)
				{
					if(i == _controls.Count)
					{
						_controls.Add(null);
					}
					
					Type? elementType = list[i]?.GetType();
					Type? controlType = _controls[i] != null ? _controls[i]!.ValueType : null;  
					
					if (elementType != controlType || _controls[i]?.ValueBindings[0].Index != i)
					{
						_controls[i] = CreateControl(elementType, i);
						_controls[i]?.InitFromSource(_sourceOwner, valueBinding.Source, valueBinding.SourceField, i);
						_controls[i]?.SetCustomLabel(i.ToString());
					}

					if(_controls[i] == null)
					{
						continue;
					}

					Gui.Text("|||");
					Gui.SameLine();

					Gui.Button("x");
					Gui.SameLine();
					
					_controls[i]!.Update();
					
				}
				Gui.Unindent();
			}

			return;
		}
	}
	
	private AnInspectorControl? CreateControl(Type? type, int index)
	{
		if (type == null)
		{
			return null;
		}

		Type? controlType = InspectorControlMap.TypeToControl(type);
		if (controlType == null)
		{
			return null;
		}
		
		return (AnInspectorControl) Activator.CreateInstance(controlType, _inspector, $"{Id}_{index}")!;
	}
}
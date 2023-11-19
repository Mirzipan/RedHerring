﻿using System.Collections;
using ImGuiNET;
using Gui = ImGuiNET.ImGui;

using System.Reflection;
using RedHerring.ImGui;
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
	private class ControlDescriptor
	{
		public AnInspectorControl? Control = null;
		public readonly string              DeleteButtonLabelId;

		public ControlDescriptor(string deleteButtonLabelId)
		{
			DeleteButtonLabelId = deleteButtonLabelId;
		}
	}

	private          object?                   _sourceOwner = null;
	private          bool                      _isReadOnly  = false;
	private readonly string                    _buttonCreateLabelId;
	private readonly List<ControlDescriptor> _controls = new();

	public InspectorListControl(Inspector inspector, string id) : base(inspector, id)
	{
		_buttonCreateLabelId = "+" + id;
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
		InspectorBinding binding = Bindings.First();
		
		object? value = binding.GetValue();
		if (value == null)
		{
			return;
		}

		// if (value is Array array) // array first, because it's a special case of IList
		// {
		// 	Type arrayElementType = binding.SourceField!.FieldType.GetElementType()!;
		// 	
		// 	if (Gui.CollapsingHeader(LabelId, ImGuiTreeNodeFlags.DefaultOpen))
		// 	{
		// 		Gui.Indent();
		//
		// 		//foreach (AnInspectorControl control in _controls)
		// 		for (int i = 0; i < array.Length; ++i)
		// 		{
		// 		}
		//
		// 		Gui.Unindent();
		// 	}
		//
		// 	return;
		// }
		
		if (value is IList list)
		{
			bool createNewElement   = false;
			int  deleteElementIndex = -1;
			
			//Type[] listElementType = binding.SourceField!.FieldType.GenericTypeArguments;

			//Gui.SetNextItemWidth(Gui.GetContentRegionAvail().X - 20);
			//if (Gui.CollapsingHeader(LabelId, ImGuiTreeNodeFlags.DefaultOpen | ImGuiTreeNodeFlags.AllowItemOverlap))
			if (Gui.TreeNodeEx(LabelId, ImGuiTreeNodeFlags.AllowItemOverlap))
			{
				createNewElement = NewElementButtonOnTheSameLine(list.IsFixedSize);

				//Gui.Indent();
			
				//foreach (AnInspectorControl control in _controls)
				for(int i = 0; i < list.Count; ++i)
				{
					// add new control
					if(i == _controls.Count)
					{
						_controls.Add(new ControlDescriptor($"x{Id}_B{i}"));
					}
					
					Type? elementType = list[i]?.GetType();
					Type? controlType = _controls[i].Control != null ? _controls[i].Control!.BoundValueType : null;  
					
					if (elementType != controlType || _controls[i].Control?.Bindings[0].Index != i)
					{
						AnInspectorControl? control = CreateControl(elementType, i);
						control?.InitFromSource(_sourceOwner, binding.Source, binding.SourceFieldInfo, i);
						control?.SetCustomLabel(i.ToString());
						_controls[i].Control = control;
					}

					if(_controls[i].Control == null)
					{
						continue;
					}

					// draggable reorder symbol
					Gui.PushStyleVar(ImGuiStyleVar.Alpha, 0.5f);
					Icon.ReorderList();
					Gui.PopStyleVar();
					Gui.SameLine();

					// delete button
					if (!list.IsFixedSize)
					{
						if (Gui.Button(_controls[i].DeleteButtonLabelId))
						{
							deleteElementIndex = i;
						}
						Gui.SameLine();
					}

					// element
					_controls[i].Control!.Update();
					
				}
				//Gui.Unindent();
				Gui.TreePop();
			}
			else
			{
				createNewElement = NewElementButtonOnTheSameLine(list.IsFixedSize);
			}

			if (createNewElement)
			{
				Console.WriteLine("Create new element");
				_inspector.Commit(new InspectorCreateListElementCommand(Bindings));
			}

			if (deleteElementIndex != -1)
			{
				Console.WriteLine($"Delete element {deleteElementIndex}");
				_inspector.Commit(new InspectorDeleteListElementCommand(Bindings, deleteElementIndex));
			}

			return;
		}
	}

	private bool NewElementButtonOnTheSameLine(bool isFixedSize)
	{
		if (isFixedSize)
		{
			return false;
		}

		Gui.SameLine();
		return IconButton.Add(ButtonSize.Small);
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
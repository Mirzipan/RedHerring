using System.Collections;
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
		public readonly string              DeleteButtonId;

		public ControlDescriptor(string deleteButtonId)
		{
			DeleteButtonId = deleteButtonId;
		}
	}

	private          object?                   _sourceOwner = null;
	private          bool                      _isReadOnly  = false;
	private readonly string                    _buttonCreateElementId;
	private readonly List<ControlDescriptor> _controls = new();

	public InspectorListControl(Inspector inspector, string id) : base(inspector, id)
	{
		_buttonCreateElementId = id + ".create";
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

		if (value is not IList list)
		{
			return;
		}

		bool createNewElement   = false;
		int  deleteElementIndex = -1;
			
		if (Gui.TreeNodeEx(LabelId, ImGuiTreeNodeFlags.AllowItemOverlap))
		{
			createNewElement = NewElementButtonOnTheSameLine(list.IsFixedSize);

			for(int i = 0; i < list.Count; ++i)
			{
				// add new control
				if(i == _controls.Count)
				{
					_controls.Add(new ControlDescriptor($"{Id}.delete{i}"));
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
					if (ButtonDeleteElement(_controls[i].DeleteButtonId))
					{
						deleteElementIndex = i;
					}
					Gui.SameLine();
				}

				// element
				_controls[i].Control!.Update();
					
			}
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
	}

	private bool NewElementButtonOnTheSameLine(bool isFixedSize)
	{
		if (isFixedSize)
		{
			return false;
		}

		Gui.SameLine();
		return ButtonCreateElement(_buttonCreateElementId);
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

	private bool ButtonCreateElement(string id)
	{
		Gui.PushID(id);
		bool result = IconButton.Add(ButtonSize.Small);
		Gui.PopID();
		return result;
	}

	private bool ButtonDeleteElement(string id)
	{
		Gui.PushID(id);
		bool result = IconButton.Remove(ButtonSize.Regular);
		Gui.PopID();
		return result;
	}
}
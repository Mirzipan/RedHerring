using System.Collections;
using ImGuiNET;
using Gui = ImGuiNET.ImGui;

using System.Reflection;
using RedHerring.Alexandria.Extensions;
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
public sealed class InspectorListControl : InspectorControl
{
	private class ControlDescriptor
	{
		public          InspectorControl? Control = null;
		public readonly string            DeleteButtonId;
		public readonly string            DragAndDropId;

		public ControlDescriptor(string deleteButtonId, string dragAndDropId)
		{
			DeleteButtonId = deleteButtonId;
			DragAndDropId  = dragAndDropId;
		}
	}

	private bool _isMultipleValues = false;
	private bool _isNullSource     = false;
	private bool _isReadOnly       = false;
	
	private readonly string                  _buttonCreateElementId;
	private readonly List<ControlDescriptor> _controls = new();

	private object? _listValue = null;

	private int _draggedIndex = -1; // to avoid using unsafe context

	public InspectorListControl(Inspector inspector, string id) : base(inspector, id)
	{
		_buttonCreateElementId = id + ".create";
	}

	public override void InitFromSource(object? sourceOwner, object source, FieldInfo? sourceField = null, int sourceIndex = -1)
	{
		base.InitFromSource(sourceOwner, source, sourceField, sourceIndex);
		_isReadOnly = sourceField != null && (sourceField.IsInitOnly || sourceField.GetCustomAttribute<ReadOnlyInInspectorAttribute>() != null);
	}

	public override void AdaptToSource(object? sourceOwner, object source, FieldInfo? sourceField = null)
	{
		base.AdaptToSource(sourceOwner, source, sourceField);
		_isReadOnly |= sourceField != null && (sourceField.IsInitOnly || sourceField.GetCustomAttribute<ReadOnlyInInspectorAttribute>() != null);
	}
	
	public override void Update()
	{
		if (SourceFieldValuesChanged())
		{
			Rebuild();
		}

		IList? list = Bindings.Count != 1 ? null : Bindings[0].GetValue() as IList;
		if (list == null)
		{
			return;
		}

		bool createNewElement   = false;
		int  deleteElementIndex = -1;
			
		if (Gui.TreeNodeEx(LabelId, ImGuiTreeNodeFlags.AllowItemOverlap))
		{
			createNewElement = NewElementButtonOnTheSameLine(list.IsFixedSize);

			for(int i = 0; i < _controls.Count; ++i)
			{
				if(_controls[i].Control == null)
				{
					continue;
				}

				// draggable reorder symbol
				CreateDragAndDropControl(_controls[i].DragAndDropId, i);
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

	private bool SourceFieldValuesChanged()
	{
		if (Bindings.Count == 0)
		{
			throw new InvalidDataException();
		}

		if (Bindings.Count > 1)
		{
			return !_isMultipleValues;
		}

		InspectorBinding binding = Bindings[0];
		
		object? value = binding.GetValue();
		if (value == null)
		{
			return !_isNullSource;
		}

		if (value is not IList list)
		{
			throw new InvalidDataException();
		}

		if (value != _listValue)
		{
			return true;
		}

		return _controls.Count != list.Count;
	}

	private void Rebuild()
	{
		Console.WriteLine($"Rebuild called on list {Id}");
		_controls.Clear();
		
		if (Bindings.Count > 1)
		{
			_isMultipleValues = true;
			return;
		}
		_isMultipleValues = false;

		InspectorBinding binding = Bindings[0];
		
		object? value = binding.GetValue();
		if (value == null)
		{
			_isNullSource = true;
			return;
		}
		_isNullSource = false;

		if (value is not IList list)
		{
			return; // error
		}
		_listValue = value;

		for(int i = 0; i < list.Count; ++i)
		{
			if(i == _controls.Count)
			{
				_controls.Add(new ControlDescriptor($"{Id}.delete[{i}]", $"{Id}.dad[{i}]"));
			}
			
			Type? elementType = list[i]              == null ? binding.GetElementType() : list[i]!.GetType();
			Type? controlType = _controls[i].Control != null ? _controls[i].Control!.BoundValueType : null;  
				
			if (elementType != controlType || _controls[i].Control?.Bindings[0].Index != i)
			{
				InspectorControl? control = CreateControl(elementType, i);
				control?.InitFromSource(binding.SourceOwner, binding.Source, binding.SourceFieldInfo, i);
				control?.SetCustomLabel(i.ToString());
				_controls[i].Control = control;
			}
		}
	}
	
	private InspectorControl? CreateControl(Type? type, int index)
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
		
		return (InspectorControl) Activator.CreateInstance(controlType, _inspector, $"{Id}[{index}]")!;
	}

	#region Drag&drop
	private void CreateDragAndDropControl(string dragAndDropId, int index)
	{
		Gui.PushID(dragAndDropId);
		
		Gui.PushStyleVar(ImGuiStyleVar.Alpha, 0.5f);
		Gui.Button(TextIcon.ReorderList);
		Gui.PopStyleVar();

		if (Gui.BeginDragDropSource(ImGuiDragDropFlags.SourceNoDisableHover))
		{
			_draggedIndex = index;
			Gui.SetDragDropPayload(Id, IntPtr.Zero, 0);
			Gui.Text("Dragging list item");
			Gui.EndDragDropSource();
		}

		if (Gui.BeginDragDropTarget())
		{
			Gui.AcceptDragDropPayload(Id);
			if (Gui.IsMouseReleased(ImGuiMouseButton.Left))
			{
				if (_draggedIndex >= 0 && _draggedIndex < _controls.Count)
				{
					Console.WriteLine($"Swap {_draggedIndex} with {index}");
					_inspector.Commit(new InspectorSwapListElementsCommand(Bindings, _draggedIndex, index));
				}

				_draggedIndex = -1;
			}
			Gui.EndDragDropTarget();
		}

		Gui.PopID();
	}
	#endregion
	
	#region Buttons
	private bool NewElementButtonOnTheSameLine(bool isFixedSize)
	{
		if (isFixedSize)
		{
			return false;
		}

		Gui.SameLine();
		return ButtonCreateElement(_buttonCreateElementId);
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
	#endregion
}
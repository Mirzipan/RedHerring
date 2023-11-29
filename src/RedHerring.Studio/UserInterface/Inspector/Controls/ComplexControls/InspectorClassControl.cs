using System.Collections;
using System.Reflection;
using ImGuiNET;
using RedHerring.Alexandria.Extensions;
using RedHerring.Studio.UserInterface.Attributes;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface;

/*
	Naming, just for clarity:

	class MyData	<-- sourceOwner:object
	{
		public MyData2 Data;       <-- source:object
		public List<MyData2> Data; <-- source:object
	}

	class MyData2
	{
		public int Value; <-- sourceFieldValue:object, sourceField:FieldInfo, sourceFieldType:Type

		[Button]
		public void MyMethod() {}
	}
*/
public sealed class InspectorClassControl : InspectorControl
{
	private          List<InspectorControl> _controls             = new();
	private          bool                   _isMultipleValues     = false;
	private          bool                   _isNullSource         = false;
	private          bool                   _allowDeleteReference = false;
	private readonly string                 _instantiationButtonId;
	private readonly string                 _instantiationPopupId;
	private readonly string                 _deleteButtonId;
	private          List<object?>          _sourceFieldValues = new();

	private Type[]? _assignableTypes = null;

	public InspectorClassControl(Inspector inspector, string id) : base(inspector, id)
	{
		_instantiationButtonId = id + ".button";
		_instantiationPopupId  = id + ".popup";
		_deleteButtonId        = id + ".delete";
	}

	#region Build
	private void Rebuild()
	{
		Console.WriteLine($"Rebuild called on class {Id}");
		
		RemoveAllControls();
		if(Bindings.Count == 0)
		{
			return;
		}
		
		CreateControlsFromSource(Bindings[0]);
		
		for(int i=1; i<Bindings.Count; ++i)
		{
			AdaptControlsToSource(Bindings[i]);
		}
	}

	private void RemoveAllControls()
	{
		_sourceFieldValues.Clear();
		_controls.Clear();
		_isMultipleValues = false;
	}

	private void CreateControlsFromSource(InspectorBinding binding)
	{
		object? sourceValue = binding.GetValue();
		_sourceFieldValues.Add(sourceValue);

		Type? boundType = binding.BoundType;
		if (sourceValue == null || boundType == null)
		{
			_isNullSource    = true;
			return;
		}

		_isNullSource         = false;
		_allowDeleteReference = binding.AllowDeleteReference;
		
		CreateControlsFromSourceFields(boundType, binding.Source, sourceValue);
		CreateControlsFromSourceMethods(boundType, binding.Source, sourceValue);
	}

	private void CreateControlsFromSourceFields(Type sourceFieldType, object source, object sourceFieldValue)
	{
		FieldInfo[] fields = sourceFieldType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		foreach (FieldInfo field in fields)
		{
			if(!IsFieldVisible(field))
			{
				continue;
			}
			
			Type? controlType = InspectorControlMap.FieldToControl(field);
			if (controlType == null)
			{
				continue; // unsupported
			}
			
			string controlId = $"{Id}.{field.Name}";
			
			InspectorControl control = (InspectorControl) Activator.CreateInstance(controlType, _inspector, controlId)!;
			_controls.Add(control);

			control.InitFromSource(source, sourceFieldValue, field);
		}
	}

	// buttons (only in first object, any other object in the same inspector removes buttons)
	private void CreateControlsFromSourceMethods(Type sourceFieldType, object source, object sourceFieldValue)
	{
		MethodInfo[] methods = sourceFieldType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		foreach (MethodInfo method in methods)
		{
			ButtonAttribute? buttonAttribute = method.GetCustomAttribute<ButtonAttribute>();
			if(buttonAttribute == null)
			{
				continue;
			}
			
			string controlId = $"{Id}.{method.Name}()";

			InspectorButtonControl button = new (_inspector, controlId, buttonAttribute.Title ?? method.Name.PrettyCamelCase(), sourceFieldValue, method);
			_controls.Add(button);
		}
	}
	#endregion

	#region Adapt
	private void AdaptControlsToSource(InspectorBinding binding)
	{
		object? sourceValue = binding.GetValue();
		_sourceFieldValues.Add(sourceValue);

		Type? boundType = binding.BoundType;
		if (sourceValue == null || boundType == null)
		{
			if (_isNullSource)
			{
				return;
			}

			_isMultipleValues = true;
			return;
		}

		_allowDeleteReference &= binding.AllowDeleteReference;

		bool[] commonControls = new bool[_controls.Count];

		AdaptControlsToSourceFields(boundType, binding.SourceOwner, binding.Source, sourceValue, commonControls);
		AdaptControlsToSourceMethods(boundType, binding.SourceOwner, binding.Source, sourceValue, commonControls);
		
		// remove all controls that are not common for all sources
		for(int i=commonControls.Length -1;i >=0;--i)
		{
			if(!commonControls[i])
			{
				_controls.RemoveAt(i);
			}
		}
	}

	private void AdaptControlsToSourceFields(Type sourceFieldType, object? sourceOwner, object source, object sourceFieldValue, bool[] commonControls)
	{
		FieldInfo[] fields = sourceFieldType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		foreach (FieldInfo field in fields)
		{
			if(!IsFieldVisible(field))
			{
				continue;
			}
			
			Type? controlType = InspectorControlMap.FieldToControl(field);
			if (controlType == null)
			{
				continue; // unsupported
			}
			
			string controlId = $"{Id}.{field.Name}";

			int controlIndex = _controls.FindIndex(x => x.Id == controlId);
			if(controlIndex == -1)
			{
				continue; // control not found => it's not common for all sources => skip
			}

			InspectorControl control = _controls[controlIndex];
			if (control is InspectorListControl)
			{
				continue; // list control cannot be used on multiple objects at once => skip
			}

			FieldInfo? firstSourceField = control.Bindings[0].SourceFieldInfo;
			if (firstSourceField == null)
			{
				continue;
			}

			if(firstSourceField.FieldType != field.FieldType)
			{
				// control type doesn't match, there is an exception for classes
				if (control is not InspectorClassControl || !field.FieldType.IsClass)
				{
					continue; // control type does not match => it's not common for all sources => skip
				}
			}

			if(!firstSourceField.GetCustomAttributes().SequenceEqual(field.GetCustomAttributes()))
			{
				continue; // attributes don't match => it's not common for all sources => skip
			}

			control.AdaptToSource(sourceOwner, sourceFieldValue, field);
			commonControls[controlIndex] = true;
		}
	}

	private void AdaptControlsToSourceMethods(Type sourceFieldType, object? sourceOwner, object source, object sourceFieldValue, bool[] commonControls)
	{
		MethodInfo[] methods = sourceFieldType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		foreach (MethodInfo method in methods)
		{
			ButtonAttribute? buttonAttribute = method.GetCustomAttribute<ButtonAttribute>();
			if(buttonAttribute == null)
			{
				continue;
			}
			
			string controlId = $"{Id}.{method.Name}()";

			int controlIndex = _controls.FindIndex(x => x.Id == controlId);
			if(controlIndex == -1)
			{
				continue; // control not found => it's not common for all sources => skip
			}

			InspectorButtonControl? button = _controls[controlIndex] as InspectorButtonControl;
			if (button == null)
			{
				continue;
			}

			string label = buttonAttribute.Title ?? method.Name.PrettyCamelCase();
			if (button.Label != label)
			{
				continue;
			}

			button.AddBinding(sourceFieldValue, method);
			commonControls[controlIndex] = true;
		}
	}
	#endregion
	
	public override void Update()
	{
		if (SourceFieldValuesChanged())
		{
			Rebuild();
		}

		if (_isNullSource)
		{
			Gui.PushStyleVar(ImGuiStyleVar.Alpha, 0.5f);
			Gui.Text("[null]");
			Gui.PopStyleVar();
			Gui.SameLine();

			Gui.Text(Label);
			Gui.SameLine();

			Gui.PushID(_instantiationButtonId);
			bool instantiationButtonClicked = Gui.SmallButton("+"); //TODO - symbol, just not like the + on list to avoid confusion
			Gui.PopID();
			
			if (instantiationButtonClicked) 
			{
				InstantiateValueOrOpenInstantiationPopup();
			}

			UpdateInstantiationPopup();
			return;
		}

		if (Label == null)
		{
			foreach (InspectorControl control in _controls)
			{
				control.Update();
			}
			return;
		}

		bool treeNodeOpen = Gui.TreeNodeEx(LabelId, ImGuiTreeNodeFlags.AllowItemOverlap);
		if (_allowDeleteReference)
		{
			Gui.PushID(_deleteButtonId);
			Gui.SameLine();
			if (Gui.SmallButton("clear reference")) // TODO - maybe a symbol, but must be clear that it's not delete
			{
				DeleteValue();
			}
			Gui.PopID();
		}

		if(treeNodeOpen)
		{
			foreach (InspectorControl control in _controls)
			{
				control.Update();
			}
			Gui.TreePop();
		}
	}

	private bool SourceFieldValuesChanged()
	{
		if (_sourceFieldValues.Count != Bindings.Count)
		{
			return true;
		}

		for (int i = 0; i < Bindings.Count; ++i)
		{
			if(Bindings[i].GetValue() != _sourceFieldValues[i])
			{
				return true;
			}
		}

		return false;
	}

	private bool IsFieldVisible(FieldInfo field)
	{
		return (field.IsPublic && field.GetCustomAttribute<HideInInspectorAttribute>() == null)
		       || field.GetCustomAttribute<ShowInInspectorAttribute>() != null;
	}

	#region Value instantiation / delete
	private void InstantiateValueOrOpenInstantiationPopup()
	{
		Type? boundType = Bindings[0].BoundType;
		if (boundType != null)
		{
			_assignableTypes ??= ObtainAllAssignableTypes(boundType);
		}

		if (_assignableTypes != null && _assignableTypes.Length == 1)
		{
			// there is just one assignable type, instantiate it, no popup needed
			InstantiateClass(_assignableTypes[0]);
			return;
		}

		OpenInstantiationPopup();
	}
	
	private void OpenInstantiationPopup()
	{
		Gui.OpenPopup(_instantiationPopupId);
	}
	
	private void UpdateInstantiationPopup()
	{
		if (Gui.BeginPopup(_instantiationPopupId))
		{
			if (_assignableTypes == null || _assignableTypes.Length == 0)
			{
				Gui.Selectable("No class available");
			}
			else
			{
				Type? selectedType = null;
				foreach(Type type in _assignableTypes)
				{
					if (Gui.Selectable(type.Name))
					{
						selectedType = type;
					}
				}

				if (selectedType != null)
				{
					InstantiateClass(selectedType);
				}
			}

			Gui.EndPopup();
		}
	}
	
	private void InstantiateClass(Type type)
	{
		_inspector.Commit(new InspectorInstantiateClassCommand(Bindings, type));
		_sourceFieldValues.Clear(); // force refresh
	}

	private Type[] ObtainAllAssignableTypes(Type baseType)
	{
		Type[] availableTypes = AppDomain.CurrentDomain.GetAssemblies()
			.SelectMany(domainAssembly => domainAssembly.GetTypes())
			.Where(type => baseType.IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface
			).ToArray();

		return availableTypes;
	}

	private void DeleteValue()
	{
		_inspector.Commit(new InspectorDeleteClassCommand(Bindings));
		_sourceFieldValues.Clear(); // force refresh
	}
	#endregion
}
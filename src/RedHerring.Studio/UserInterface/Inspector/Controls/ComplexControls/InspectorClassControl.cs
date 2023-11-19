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
public sealed class InspectorClassControl : AnInspectorControl
{
	private          List<AnInspectorControl> _controls         = new();
	private          bool                     _isMultipleValues = false;
	private          bool                     _isNullSource     = false;
	private readonly string                   _createValuePopupId;

	private Type?   _sourceFieldType;
	private Type[]? _assignableTypes = null;

	public InspectorClassControl(Inspector inspector, string id) : base(inspector, id)
	{
		_createValuePopupId = id + ".popup";
	}
	
	#region Init
	public override void InitFromSource(object? sourceOwner, object source, FieldInfo? sourceField = null, int sourceIndex = -1)
	{
		base.InitFromSource(sourceOwner, source, sourceField, sourceIndex);

		_isMultipleValues = false;
		
		object? sourceFieldValue = sourceField == null ? source : sourceField.GetValue(source);
		if (sourceFieldValue == null)
		{
			_sourceFieldType = sourceField?.FieldType;
			_isNullSource    = true;
			return;
		}

		_sourceFieldType = sourceFieldValue.GetType(); // this should properly handle abstract bases

		// binding to item inside list/array
		if (sourceIndex != -1)
		{
			if (_sourceFieldType.IsArray)
			{
				_sourceFieldType = _sourceFieldType.GetElementType()!;
			}
			else if (_sourceFieldType.IsGenericType && _sourceFieldType.GetGenericTypeDefinition() == typeof(List<>))
			{
				_sourceFieldType = _sourceFieldType.GetGenericArguments()[0];
			}

			object? sourceElement = (sourceFieldValue as IList)?[sourceIndex];
			if (sourceElement == null)
			{
				_isNullSource = true;
				return;
			}

			sourceFieldValue = sourceElement;
			sourceIndex      = -1;
		}

		InitFromSourceFields(_sourceFieldType, source, sourceFieldValue, sourceIndex);
		InitFromSourceMethods(_sourceFieldType, source, sourceFieldValue);
	}

	private void InitFromSourceFields(Type sourceFieldType, object source, object sourceFieldValue, int sourceIndex)
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
			
			AnInspectorControl control = (AnInspectorControl) Activator.CreateInstance(controlType, _inspector, controlId)!;
			_controls.Add(control);

			control.InitFromSource(source, sourceFieldValue, field, sourceIndex);
		}
	}

	// buttons (only in first object, any other object in the same inspector removes buttons)
	private void InitFromSourceMethods(Type sourceFieldType, object source, object sourceFieldValue)
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
	public override void AdaptToSource(object? sourceOwner, object source, FieldInfo? sourceField = null)
	{
		base.AdaptToSource(sourceOwner, source, sourceField);
		
		object? sourceFieldValue = sourceField == null ? source : sourceField.GetValue(source);
		if (sourceFieldValue == null)
		{
			if (_isNullSource)
			{
				return;
			}

			_isMultipleValues = true;
			return;
		}

		Type sourceFieldType = sourceFieldValue.GetType(); // this should properly handle abstract bases

		bool[] commonControls = new bool[_controls.Count];

		AdaptToSourceFields(sourceFieldType, sourceOwner, source, sourceFieldValue, commonControls);
		AdaptToSourceMethods(sourceFieldType, sourceOwner, source, sourceFieldValue, commonControls);
		
		// remove all controls that are not common for all sources
		for(int i=commonControls.Length-1;i>=0;--i)
		{
			if(!commonControls[i])
			{
				_controls.RemoveAt(i);
			}
		}
	}

	private void AdaptToSourceFields(Type sourceFieldType, object? sourceOwner, object source, object sourceFieldValue, bool[] commonControls)
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

			AnInspectorControl control          = _controls[controlIndex];
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

	private void AdaptToSourceMethods(Type sourceFieldType, object? sourceOwner, object source, object sourceFieldValue, bool[] commonControls)
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
		if (_isNullSource)
		{
			Gui.PushStyleVar(ImGuiStyleVar.Alpha, 0.5f);
			Gui.Text("[null]");
			Gui.PopStyleVar();
			Gui.SameLine();

			Gui.Text(Label);
			Gui.SameLine();

			if (Gui.SmallButton("+")) //TODO - symbol, just not like the + on list to avoid confusion
			{
				Gui.OpenPopup(_createValuePopupId);
			}

			UpdateCreationPopup();
			return;
		}

		if (Label == null)
		{
			foreach (AnInspectorControl control in _controls)
			{
				control.Update();
			}
			return;
		}

		//if (Gui.CollapsingHeader(Label, ImGuiTreeNodeFlags.DefaultOpen))
		if(Gui.TreeNode(LabelId))
		{
			//Gui.Indent();
			foreach (AnInspectorControl control in _controls)
			{
				control.Update();
			}
			//Gui.Unindent();
			Gui.TreePop();
		}
	}

	private bool IsFieldVisible(FieldInfo field)
	{
		return (field.IsPublic && field.GetCustomAttribute<HideInInspectorAttribute>() == null)
		       || field.GetCustomAttribute<ShowInInspectorAttribute>() != null;
	}

	private void UpdateCreationPopup()
	{
		if (Gui.BeginPopup(_createValuePopupId))
		{
			if (_sourceFieldType != null)
			{
				_assignableTypes ??= ObtainAllAssignableTypes(_sourceFieldType);
			}

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
		
		// TODO - item is not updated! .. needs rework :-(
	}

	private Type[] ObtainAllAssignableTypes(Type baseType)
	{
		Type[] availableTypes = AppDomain.CurrentDomain.GetAssemblies()
			.SelectMany(domainAssembly => domainAssembly.GetTypes())
			.Where(type => baseType.IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface
		).ToArray();

		return availableTypes;
	}
}
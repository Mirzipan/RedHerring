using System.Reflection;
using ImGuiNET;
using RedHerring.Alexandria.Extensions;
using RedHerring.Studio.UserInterface.Attributes;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface;

public sealed class InspectorClassControl : AnInspectorControl
{
	private List<AnInspectorControl> _controls = new();

	public InspectorClassControl(Inspector inspector, string id) : base(inspector, id)
	{
	}
	
	#region Init
	public override void InitFromSource(object? sourceOwner, object source, FieldInfo? sourceField = null, int sourceIndex = -1)
	{
		base.InitFromSource(sourceOwner, source, sourceField, sourceIndex);

		object? boundObject = sourceField == null ? source : sourceField.GetValue(source);
		if (boundObject == null)
		{
			return;
		}

		Type sourceType = boundObject.GetType(); // this should properly handle abstract bases
		//Type sourceType = sourceField != null ? sourceField.FieldType : source.GetType();

		InitFromSourceFields(sourceType, source, boundObject);
		InitFromSourceMethods(sourceType, source, boundObject);
	}

	private void InitFromSourceFields(Type sourceType, object source, object boundObject)
	{
		FieldInfo[] fields = sourceType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
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

			control.InitFromSource(source, boundObject, field);
		}
	}

	// buttons (only in first object, any other object in the same inspector removes buttons)
	private void InitFromSourceMethods(Type sourceType, object source, object boundObject)
	{
		MethodInfo[] methods = sourceType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		foreach (MethodInfo method in methods)
		{
			ButtonAttribute? buttonAttribute = method.GetCustomAttribute<ButtonAttribute>();
			if(buttonAttribute == null)
			{
				continue;
			}
			
			string controlId = $"{Id}.{method.Name}()";

			InspectorButtonControl button = new (_inspector, controlId, buttonAttribute.Title ?? method.Name.PrettyCamelCase(), boundObject, method);
			_controls.Add(button);
		}
	}
	#endregion

	#region Adapt
	public override void AdaptToSource(object? sourceOwner, object source, FieldInfo? sourceField = null)
	{
		// TODO - refactor, similar to InitFromSource
		base.AdaptToSource(sourceOwner, source, sourceField);
		
		object? boundObject = sourceField == null ? source : sourceField.GetValue(source);
		if (boundObject == null)
		{
			return;
		}

		Type sourceType = boundObject.GetType(); // this should properly handle abstract bases
		//Type sourceType = sourceField != null ? sourceField.FieldType : source.GetType();

		bool[] commonControls = new bool[_controls.Count];

		AdaptToSourceFields(sourceType, sourceOwner, source, boundObject, commonControls);
		AdaptToSourceMethods(sourceType, sourceOwner, source, boundObject, commonControls);
		
		// remove all controls that are not common for all sources
		for(int i=commonControls.Length-1;i>=0;--i)
		{
			if(!commonControls[i])
			{
				_controls.RemoveAt(i);
			}
		}
	}

	private void AdaptToSourceFields(Type sourceType, object? sourceOwner, object source, object boundObject, bool[] commonControls)
	{
		FieldInfo[] fields = sourceType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
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

			FieldInfo?         firstSourceField = control.ValueBindings[0].SourceField;
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

			control.AdaptToSource(sourceOwner, boundObject, field);
			commonControls[controlIndex] = true;
		}
	}

	private void AdaptToSourceMethods(Type sourceType, object? sourceOwner, object source, object boundObject, bool[] commonControls)
	{
		MethodInfo[] methods = sourceType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
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

			button.AddBinding(boundObject, method);
			commonControls[controlIndex] = true;
		}
	}
	#endregion
	
	public override void Update()
	{
		if (Label == null)
		{
			foreach (AnInspectorControl control in _controls)
			{
				control.Update();
			}
			return;
		}

		if (Gui.CollapsingHeader(Label, ImGuiTreeNodeFlags.DefaultOpen))
		{
			Gui.Indent();
			foreach (AnInspectorControl control in _controls)
			{
				control.Update();
			}
			Gui.Unindent();
		}
	}

	private bool IsFieldVisible(FieldInfo field)
	{
		return (field.IsPublic && field.GetCustomAttribute<HideInInspectorAttribute>() == null)
		       || field.GetCustomAttribute<ShowInInspectorAttribute>() != null;
	}
}
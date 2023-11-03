using System.Reflection;
using ImGuiNET;
using RedHerring.Studio.UserInterface.Attributes;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface;

public sealed class InspectorClassControl : AnInspectorControl
{
	private List<AnInspectorControl> _controls = new();

	public InspectorClassControl(Inspector inspector, string id) : base(inspector, id)
	{
	}
	
	public override void InitFromSource(object? sourceOwner, object source, FieldInfo? sourceField = null)
	{
		base.InitFromSource(sourceOwner, source, sourceField);

		object? boundObject = sourceField == null ? source : sourceField.GetValue(source);
		if (boundObject == null)
		{
			return;
		}

		Type sourceType = boundObject.GetType(); // this should properly handle abstract bases
		//Type sourceType = sourceField != null ? sourceField.FieldType : source.GetType();
		
		FieldInfo[] fields = sourceType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

		foreach (FieldInfo field in fields)
		{
			if(!IsFieldVisible(field))
			{
				continue;
			}
			
			Type? controlType = FieldToControl(field);
			if (controlType == null)
			{
				continue; // unsupported
			}
			
			string fieldId = $"{Id}.{field.Name}";
			
			AnInspectorControl control = (AnInspectorControl) Activator.CreateInstance(controlType, _inspector, fieldId)!;
			_controls.Add(control);

			control.InitFromSource(source, boundObject, field);
		}
	}

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

		FieldInfo[] fields = sourceType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		foreach (FieldInfo field in fields)
		{
			if(!IsFieldVisible(field))
			{
				continue;
			}
			
			Type? controlType = FieldToControl(field);
			if (controlType == null)
			{
				continue; // unsupported
			}
			
			string fieldId = $"{Id}.{field.Name}";

			int controlIndex = _controls.FindIndex(x => x.Id == fieldId);
			if(controlIndex == -1)
			{
				continue; // control not found => it's not common for all sources => skip
			}

			AnInspectorControl control          = _controls[controlIndex];
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
		
		// remove all controls that are not common for all sources
		for(int i=commonControls.Length-1;i>=0;--i)
		{
			if(!commonControls[i])
			{
				_controls.RemoveAt(i);
			}
		}
	}

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
using System.Reflection;
using RedHerring.Studio.UserInterface.Attributes;

namespace RedHerring.Studio.UserInterface;

public static class InspectorControlMap
{
	private static Dictionary<Type, Type> _fieldToControlMap
		= new()
		  {
			  {typeof(int), typeof(InspectorIntControl)},
			  {typeof(float), typeof(InspectorFloatControl)},
			  {typeof(string), typeof(InspectorStringControl)},
			  {typeof(bool), typeof(InspectorBoolControl)},
		  };

	private static readonly Type _classControl               = typeof(InspectorClassControl);
	private static readonly Type _enumControl                = typeof(InspectorEnumControl);
	private static readonly Type _valueDropdownIntControl    = typeof(InspectorValueDropdownIntControl);
	private static readonly Type _valueDropdownStringControl = typeof(InspectorValueDropdownStringControl);
	private static readonly Type _listControl                = typeof(InspectorListControl);

	public static Type? FieldToControl(FieldInfo fieldInfo)
	{
		Type fieldType = fieldInfo.FieldType;
		
		// by attribute
		if(fieldInfo.GetCustomAttribute<ValueDropdownAttribute>() != null)
		{
			if (fieldType == typeof(int))
			{
				return _valueDropdownIntControl;
			}

			if (fieldType == typeof(string))
			{
				return _valueDropdownStringControl;
			}

			return null;
		}
		
		// by type
		return TypeToControl(fieldType);
	}

	public static Type? TypeToControl(Type type)
	{
		if (type.IsArray || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>)))
		{
			return _listControl;
		}

		if (_fieldToControlMap.TryGetValue(type, out Type? controlType))
		{
			return controlType;
		}

		if (type.IsEnum)
		{
			return _enumControl;
		}

		if (type.IsClass)
		{
			return _classControl;
		}

		return null;
	}
}
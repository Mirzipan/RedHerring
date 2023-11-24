using System.Collections;
using System.Reflection;

namespace RedHerring.Studio.UserInterface;

public abstract class InspectorBinding
{
	public abstract bool       IsUnbound       { get; }
	public abstract bool       IsReadOnly      { get; }
	public abstract object?    SourceOwner     { get; }
	public abstract object     Source          { get; }
	public abstract FieldInfo? SourceFieldInfo { get; }
	public abstract int        Index           { get; }

	public abstract Type? BoundType { get; }

	public abstract object? GetValue();
	public abstract void SetValue(object? value);

	public static InspectorBinding Create(object? sourceOwner, object? source, FieldInfo? sourceField, int sourceIndex, Action? onCommitValue)
	{
		if (sourceIndex != -1)
		{
			return new InspectorListValueBinding(sourceOwner, source, sourceField, onCommitValue, sourceIndex);
		}

		return new InspectorValueBinding(sourceOwner, source, sourceField, onCommitValue);
	}

	public Type? GetElementType()
	{
		Type? sourceFieldType = SourceFieldInfo?.FieldType;
		if (sourceFieldType == null)
		{
			return null;
		}

		if (sourceFieldType.IsArray)
		{
			return sourceFieldType.GetElementType()!;
		}
		
		if (sourceFieldType.IsGenericType && sourceFieldType.GetGenericTypeDefinition() == typeof(List<>))
		{
			return sourceFieldType.GetGenericArguments()[0];
		}

		return null;
	}

	public object? GetFieldValue()
	{
		object? sourceFieldValue = SourceFieldInfo == null ? Source : SourceFieldInfo.GetValue(Source);
		if (sourceFieldValue == null)
		{
			return null;
		}

		//Type sourceFieldType = sourceFieldValue.GetType(); // this should properly handle abstract bases

		// binding to item inside list/array
		if (Index == -1)
		{
			return sourceFieldValue;
		}

		// if (sourceFieldType.IsArray)
		// {
		// 	sourceFieldType = sourceFieldType.GetElementType()!;
		// }
		// else if (sourceFieldType.IsGenericType && sourceFieldType.GetGenericTypeDefinition() == typeof(List<>))
		// {
		// 	sourceFieldType = sourceFieldType.GetGenericArguments()[0];
		// }

		object? sourceElement = (sourceFieldValue as IList)?[Index];
		if (sourceElement == null)
		{
			return null;
		}

		sourceFieldValue = sourceElement;
		return sourceFieldValue;
	}
}
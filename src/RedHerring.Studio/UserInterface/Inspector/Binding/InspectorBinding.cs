using System.Reflection;

namespace RedHerring.Studio.UserInterface;

// root object binding
public class InspectorBinding
{
	public readonly object Source;

	public virtual bool       IsUnbound            => false;
	public virtual bool       IsReadOnly           => false;
	public virtual bool       AllowDeleteReference => false;
	public virtual object?    SourceOwner          => null;
	public virtual FieldInfo? SourceFieldInfo      => null;
	public virtual int        Index                => -1;

	public virtual Type? BoundType => Source.GetType();

	public virtual object? GetValue()              => Source;
	public virtual void    SetValue(object? value) => throw new InvalidOperationException("Cannot set value on readonly binding");

	public InspectorBinding(object source)
	{
		Source = source;
	}
	
	public static InspectorBinding Create(object? sourceOwner, object? source, FieldInfo? sourceField, int sourceIndex, Action? onCommitValue)
	{
		if (source == null)
		{
			throw new InvalidOperationException("Source cannot be null!");
		}

		if (sourceField == null)
		{
			return new InspectorBinding(source);
		}

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
}
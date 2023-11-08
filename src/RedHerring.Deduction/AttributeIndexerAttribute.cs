namespace RedHerring.Deduction;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public class AttributeIndexerAttribute : Attribute
{
    public Type AttributeType { get; }

    public AttributeIndexerAttribute(Type attributeType)
    {
        AttributeType = attributeType;
    }
}
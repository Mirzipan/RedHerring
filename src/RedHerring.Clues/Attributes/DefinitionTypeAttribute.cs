namespace RedHerring.Clues;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public sealed class DefinitionTypeAttribute : Attribute
{
    public Type IndexedType { get; set; }

    public DefinitionTypeAttribute(Type indexedType)
    {
        IndexedType = indexedType;
    }
}
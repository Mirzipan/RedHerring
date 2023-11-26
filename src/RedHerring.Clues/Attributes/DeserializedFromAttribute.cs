namespace RedHerring.Clues;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class DeserializedFromAttribute : Attribute
{
    public Type SerializedType { get; set; }

    public DeserializedFromAttribute(Type serializedType)
    {
        SerializedType = serializedType;
    }
}
using System.Reflection;
using RedHerring.Infusion.Delegates;

namespace RedHerring.Infusion.Indexer;

internal sealed class MemberDescription
{
    public readonly Type Type;
    public readonly MemberSetter Setter;

    public MemberDescription(Type type, MemberSetter setter)
    {
        Type = type;
        Setter = setter;
    }

    public MemberDescription(FieldInfo field)
    {
        Type = field.FieldType;
        Setter = field.SetValue;
    }

    public MemberDescription(PropertyInfo property)
    {
        Type = property.PropertyType;
        Setter = (target, value) => property.SetValue(target, value, null);
    }
}
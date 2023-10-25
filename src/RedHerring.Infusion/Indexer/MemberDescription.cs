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
}
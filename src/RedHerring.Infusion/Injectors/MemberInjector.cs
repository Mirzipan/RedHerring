using RedHerring.Infusion.Exceptions;
using RedHerring.Infusion.Indexer;

namespace RedHerring.Infusion.Injectors;

internal static class MemberInjector
{
    public static void InjectMany(MemberDescription[] members, object instance, InjectionContainer container)
    {
        for (int i = 0; i < members.Length; i++)
        {
            Inject(members[i], instance, container);
        }
    }
        
    private static void Inject(MemberDescription member, object instance, InjectionContainer container)
    {
        try
        {
            member.Setter(instance, container.Resolve(member.Type));
        }
        catch (Exception e)
        {
            throw new MemberInjectionFailedException(instance.GetType(), member.Type, e);
        }
    }
}
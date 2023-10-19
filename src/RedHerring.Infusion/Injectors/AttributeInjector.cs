using RedHerring.Infusion.Indexer;

namespace RedHerring.Infusion.Injectors;

public static class AttributeInjector
{
    public static void Inject(object obj, InjectionContainer container)
    {
        var info = InjectionIndexer.Index(obj.GetType());
        MemberInjector.InjectMany(info.Members, obj, container);
    }
}
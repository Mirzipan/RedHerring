using System.Buffers;
using RedHerring.Infusion.Exceptions;
using RedHerring.Infusion.Indexer;

namespace RedHerring.Infusion.Injectors;

internal static class MethodInjector
{
    public static void InjectMany(MethodDescription[] methods, object instance, InjectionContainer container)
    {
        for (var i = 0; i < methods.Length; i++)
        {
            Inject(methods[i], instance, container);
        }
    }
        
    private static void Inject(MethodDescription method, object instance, InjectionContainer container)
    {
        object[] arguments = ArrayPool<object>.Shared.Rent(method.Parameters.Length);

        for (var i = 0; i < method.Parameters.Length; i++)
        {
            arguments[i] = container.Resolve(method.Parameters[i]);
        }

        try
        {
            method.Action(instance, arguments);
        }
        catch (Exception e)
        {
            throw new MethodInjectionFailedException(instance.GetType(), method.Name, e);
        }
        finally
        {
            ArrayPool<object>.Shared.Return(arguments);
        }
    }
}
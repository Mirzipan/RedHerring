using System.Buffers;
using RedHerring.Infusion.Exceptions;
using RedHerring.Infusion.Indexer;

namespace RedHerring.Infusion.Injectors;

public static class ConstructorInjector
{
    public static object Construct(Type contract, InjectionContainer container)
    {
        var description = InjectionIndexer.Index(contract);
        var parameters = description.DefaultConstructor.Parameters;
        object[] arguments = ArrayPool<object>.Shared.Rent(parameters.Length);

        for (var i = 0; i < parameters.Length; i++)
        {
            arguments[i] = container.Resolve(parameters[i]);
        }

        try
        {
            return description.DefaultConstructor.Activator.Invoke(arguments);
        }
        catch (Exception e)
        {
            throw new FailedToConstructException(contract, e);
        }
        finally
        {
            ArrayPool<object>.Shared.Return(arguments);
        }
    }
}
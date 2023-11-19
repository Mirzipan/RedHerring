namespace RedHerring.Infusion;

public static class ContainerExtensions
{
    public static bool TryResolve<TContract>(this InjectionContainer @this, out TContract? instance)
    {
        if (@this.HasBinding<TContract>())
        {
            instance = @this.Resolve<TContract>();
            return true;
        }

        instance = default;
        return false;
    }
    
    public static bool TryResolve(this InjectionContainer @this, Type contract, out object? instance)
    {
        if (@this.HasBinding(contract))
        {
            instance = @this.Resolve(contract);
            return true;
        }

        instance = default;
        return false;
    }
}
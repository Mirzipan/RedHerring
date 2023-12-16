namespace RedHerring.Infusion;

public static class DescriptionExtensions
{
    public static ContainerDescription AddSingleton<TContract>(this ContainerDescription @this)
    {
        return @this.AddSingleton(typeof(TContract));
    }
    public static ContainerDescription AddSingletonAsInterfaces(this ContainerDescription @this, Type concrete)
    {
        var interfaces = concrete.GetInterfaces();
        return @this.AddSingleton(concrete, interfaces);
    }
    public static ContainerDescription AddSingletonWithInterfaces(this ContainerDescription @this, Type concrete)
    {
        var interfaces = concrete.GetInterfaces();
        var types = new Type[interfaces.Length + 1];
        types[^1] = concrete;
        return @this.AddSingleton(concrete, types);
    }
    
    public static ContainerDescription AddTransient<TContract>(this ContainerDescription @this)
    {
        return @this.AddTransient(typeof(TContract));
    }
    
    public static ContainerDescription AddTransientAsInterfaces(this ContainerDescription @this, Type concrete)
    {
        var interfaces = concrete.GetInterfaces();
        return @this.AddTransient(concrete, interfaces);
    }
    public static ContainerDescription AddTransientWithInterfaces(this ContainerDescription @this, Type concrete)
    {
        var interfaces = concrete.GetInterfaces();
        var types = new Type[interfaces.Length + 1];
        types[^1] = concrete;
        return @this.AddTransient(concrete, types);
    }
    
    public static ContainerDescription AddSingleton<TContract>(this ContainerDescription @this, object instance)
    {
        return @this.AddSingleton(instance, typeof(TContract));
    }
    
    public static ContainerDescription AddSingletonAsInterfaces(this ContainerDescription @this, object instance)
    {
        var interfaces = instance.GetType().GetInterfaces();
        return @this.AddSingleton(instance, interfaces);
    }
    public static ContainerDescription AddSingletonWithInterfaces(this ContainerDescription @this, object instance)
    {
        var type = instance.GetType();
        var interfaces = type.GetInterfaces();
        var types = new Type[interfaces.Length + 1];
        types[^1] = type;
        return @this.AddSingleton(instance, types);
    }
}
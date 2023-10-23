namespace RedHerring.Core.Systems;

public sealed class SystemReference
{
    public readonly Type Type;
    public readonly object? Config;

    public static SystemReference Create<T>(object? config = null) where T : AnEngineSystem
    {
        return new SystemReference(typeof(T), config);
    }

    public static SystemReference Create(Type type, object? config = null)
    {
        if (!type.IsSubclassOf(typeof(AnEngineSystem)))
        {
            throw new ArgumentException($"type needs to be derived from {nameof(AnEngineSystem)}");
        }
        
        return new SystemReference(type, config);
    }
    
    private SystemReference(Type type, object? config = null)
    {
        Type = type;
        Config = config;
    }
}
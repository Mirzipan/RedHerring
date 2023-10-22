namespace RedHerring.Core.Systems;

public class SystemReference
{
    public Type Type;
    public object? Config;

    public SystemReference(Type type, object? config = null)
    {
        if (!type.IsSubclassOf(typeof(AnEngineSystem)))
        {
            throw new ArgumentException($"type needs to be derived from {nameof(AnEngineSystem)}");
        }
        
        Type = type;
        Config = config;
    }
}
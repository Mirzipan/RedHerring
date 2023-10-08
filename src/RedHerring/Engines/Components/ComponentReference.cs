namespace RedHerring.Engines.Components;

public class ComponentReference
{
    public Type Type;
    public object? Config;

    public ComponentReference(Type type, object? config = null)
    {
        if (!type.IsSubclassOf(typeof(AnEngineComponent)))
        {
            throw new ArgumentException($"type needs to be derived from {nameof(AnEngineComponent)}");
        }
        
        Type = type;
        Config = config;
    }
}
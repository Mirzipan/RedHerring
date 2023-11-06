namespace RedHerring.Studio.Models.Project.Processors;

public sealed class ProcessorRegistry
{
    private static readonly Type GenericProcessorType = typeof(AssetProcessor<,>);
    private readonly record struct Key(Type InputType, Type OutputType);
    
    private Dictionary<Key, Processor> _byKey = new();
    private Processor _fallback = new CopyProcessor();

    #region Manipulation

    public void Register<T>(T processor) where T : Processor
    {
        var type = typeof(T);
        var baseType = GetGenericImporterType(type);
        if (baseType is null)
        {
            return;
        }

        var args = baseType.GetGenericArguments();
        var input = args[0];
        var output = args[1];
        var key = new Key(input, output);
        
        if (_byKey.ContainsKey(key))
        {
            // TODO: log?
        }

        _byKey[key] = processor;
    }

    public void Unregister<TInput, TOutput>() => Unregister(typeof(TInput), typeof(TOutput));
    
    public void Unregister(Type input, Type output) => _byKey.Remove(new Key(input, output));

    #endregion Manipulation

    #region Queries

    public Processor Find<TInput, TOutput>() => Find(typeof(TInput), typeof(TOutput));

    public Processor Find(Type input, Type output)
    {
        return _byKey.TryGetValue(new Key(input, output), out var result) ? result : _fallback;
    }

    #endregion Queries
    
    #region Private

    private static Type? GetGenericImporterType(Type type)
    {
        Type? current = type;
        while (current is not null)
        {
            if (current.IsGenericType && current.GetGenericTypeDefinition() == GenericProcessorType)
            {
                return current;
            }

            current = current.BaseType;
        }

        return null;
    }

    #endregion Private
}
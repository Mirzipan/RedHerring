using System.Reflection;
using RedHerring.Alexandria.Extensions.Reflection;
using RedHerring.Deduction;

namespace RedHerring.Clues;

public sealed class DefinitionIndexer : IIndexTypes, IDisposable
{
    private static readonly Type BaseType = typeof(Definition);
    private static readonly Type SerializedBaseType = typeof(SerializedDefinition);
    
    private readonly Dictionary<Type, Type> _serializedDefinitionToDefinitionMap = new();

    #region Lifecycle

    public void Index(Type type)
    {
        if (!type.IsSubclassOf(BaseType) || !type.HasDefaultConstructor())
        {
            return;
        }

        var attributes = type.GetCustomAttributes<DeserializedFromAttribute>();
        foreach (var attribute in attributes)
        {
            if (!attribute.SerializedType.IsSubclassOf(SerializedBaseType))
            {
                continue;
            }

            _serializedDefinitionToDefinitionMap[attribute.SerializedType] = type;
        }
    }
    
    public void Dispose()
    {
        _serializedDefinitionToDefinitionMap.Clear();
    }

    #endregion Lifecycle

    #region Queries

    public Type? DefinitionType(Type serializedType)
    {
        return _serializedDefinitionToDefinitionMap.TryGetValue(serializedType, out var result) ? result : null;
    }

    #endregion Queries
}
using System.Reflection;
using System.Runtime.CompilerServices;
using RedHerring.Alexandria.Collections;
using RedHerring.Alexandria.Extensions.Reflection;
using RedHerring.Deduction.Exceptions;

namespace RedHerring.Deduction;

public sealed class MetadataDatabase : IDisposable
{
    private static readonly Type AttributeIndexerType = typeof(IIndexAttributes);
    private static readonly Type TypeIndexerType = typeof(IIndexTypes);

    private readonly Dictionary<Type, IIndexMetadata> _indexersByType = new();
    private readonly ListDictionary<Type, IIndexAttributes> _attributeIndexers = new();
    private readonly List<IIndexTypes> _typeIndexers = new();
    private readonly AssemblyCollection _container;

    public IEnumerable<IIndexMetadata> Indexers => _indexersByType.Values;

    #region Lifecycle

    public MetadataDatabase(AssemblyCollection container)
    {
        _container = container;
    }

    public void Process()
    {
        GatherIndexers();
        foreach (var type in _container.GetAllTypes())
        {
            Index(type);
        }
    }

    public void Dispose()
    {
        _container.Dispose();
        _indexersByType.Clear();
        _attributeIndexers.Clear();
        _typeIndexers.Clear();
    }

    #endregion Lifecycle

    #region Private

    private void GatherIndexers()
    {
        foreach (var type in _container.GetAllTypes())
        {
            if (type.IsInterface)
            {
                continue;
            }
            
            var attributes = type.GetCustomAttributes<AttributeIndexerAttribute>();
            foreach (var attribute in attributes)
            {
                if (!type.IsAssignableTo(AttributeIndexerType))
                {
                    continue;
                }
                
                AddAttributeIndexer(attribute.AttributeType, type);
            }
            
            if (type.IsAssignableTo(TypeIndexerType))
            {
                AddTypeIndexer(type);
            }
        }
    }

    private void AddTypeIndexer(Type indexerType)
    {
        if (!indexerType.HasDefaultConstructor())
        {
            throw new NoDefaultConstructionException(indexerType);
        }

        var indexer = GetOrCreate(indexerType);
        if (indexer is not null)
        {
            _typeIndexers.Add((IIndexTypes)indexer);
        }
    }

    private void AddAttributeIndexer(Type attributeType, Type indexerType)
    {
        if (!indexerType.HasDefaultConstructor())
        {
            throw new NoDefaultConstructionException(indexerType);
        }

        var indexer = GetOrCreate(indexerType);
        if (indexer is not null)
        {
            _attributeIndexers.Add(attributeType, (IIndexAttributes)indexer);
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Index(Type type)
    {
        for (int i = 0; i < _typeIndexers.Count; i++)
        {
            try
            {
                _typeIndexers[i].Index(type);
            }
            catch
            {
                // log maybe?
            }
        }

        var attributes = type.GetCustomAttributes();
        foreach (var attribute in attributes)
        {
            var attributeType = attribute.GetType();
            if (!_attributeIndexers.TryGet(attributeType, out var indexers))
            {
                continue;
            }

            foreach (var indexer in indexers)
            {
                try
                {
                    indexer.Index(attribute, type);
                }
                catch
                {
                    // log maybe?
                }
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private IIndexMetadata? GetOrCreate(Type type)
    {
        if (_indexersByType.TryGetValue(type, out var indexer))
        {
            return indexer;
        }

        try
        {
            indexer = (IIndexMetadata?)Activator.CreateInstance(type);
        }
        catch
        {
            // log maybe?
            return null;
        }

        _indexersByType[type] = indexer!;
        return indexer;
    }

    #endregion Private
}
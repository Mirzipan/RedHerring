using System.Reflection;
using System.Runtime.CompilerServices;
using RedHerring.Alexandria.Collections;
using RedHerring.Alexandria.Extensions.Reflection;
using RedHerring.Deduction.Exceptions;

namespace RedHerring.Deduction;

public sealed class FindingsContext : IDisposable
{
    private static readonly Type AttributeIndexerType = typeof(AttributeIndexer);
    private static readonly Type TypeIndexerType = typeof(TypeIndexer);

    private readonly Dictionary<Type, MetadataIndexer> _indexersByType = new();
    private readonly ListDictionary<Type, AttributeIndexer> _attributeIndexers = new();
    private readonly List<TypeIndexer> _typeIndexers = new();
    private readonly AssemblyCollection _container;

    public IEnumerable<MetadataIndexer> Indexers => _indexersByType.Values;

    #region Lifecycle

    internal FindingsContext(AssemblyCollection container)
    {
        _container = container;
    }

    internal void Process()
    {
        GatherIndexers();
        foreach (var type in _container.GetAllTypes())
        {
            Index(type);
        }
    }

    void IDisposable.Dispose()
    {
        _container.Dispose();
        _indexersByType.Clear();
        _attributeIndexers.Clear();
        _typeIndexers.Clear();
    }

    #endregion Lifecycle

    #region Queries

    public T? IndexerByType<T>() where T : MetadataIndexer
    {
        var indexer = IndexerByType(typeof(T));
        return indexer is not null ? (T)indexer : default;
    }

    public MetadataIndexer? IndexerByType(Type type) => _indexersByType.GetValueOrDefault(type);

    #endregion Queries

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
            _typeIndexers.Add((TypeIndexer)indexer);
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
            _attributeIndexers.Add(attributeType, (AttributeIndexer)indexer);
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
    private MetadataIndexer? GetOrCreate(Type type)
    {
        if (_indexersByType.TryGetValue(type, out var indexer))
        {
            return indexer;
        }

        try
        {
            indexer = (MetadataIndexer?)Activator.CreateInstance(type);
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
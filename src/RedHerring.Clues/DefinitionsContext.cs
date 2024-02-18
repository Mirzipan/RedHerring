﻿using RedHerring.Deduction;

namespace RedHerring.Clues;

public sealed class DefinitionsContext : IDisposable
{
    private readonly Dictionary<Type, Dictionary<DefinitionId, Definition>> _data = new();

    // TODO(Mirzi): replace with a pooled list instead
    private static readonly List<Type> TmpIndexedTypes = new List<Type>(16);
    
    internal DefinitionsContext()
    {
    }
    
    #region Manipulation
        
    /// <summary>
    /// Adds the specified definition. In case there already is one with the same indexed type and id, this will overwrite it
    /// </summary>
    /// <param name="definition"></param>
    public void Add(Definition definition)
    {
        Type type = definition.GetType();
        AddDefinition(definition, type);

        var indexer = Findings.IndexerByType<DefinitionTypeIndexer>();
        if (indexer is null)
        {
            return;
        }
        
        // TODO(Mirzi): replace with a pooled list instead
        TmpIndexedTypes.Clear();
        indexer.IndexedAs(type, TmpIndexedTypes);
        foreach (var entry in TmpIndexedTypes)
        {
            AddDefinition(definition, entry);
        }
    }

    /// <summary>
    /// Removes the specified definition.
    /// </summary>
    /// <param name="definition"></param>
    /// <returns></returns>
    public bool Remove(Definition definition)
    {
        Type type = definition.GetType();
        bool result = RemoveDefinition(definition, type);
            
        var indexer = Findings.IndexerByType<DefinitionTypeIndexer>();
        if (indexer is null)
        {
            return result;
        }
        
        // TODO(Mirzi): replace with a pooled list instead
        TmpIndexedTypes.Clear();
        indexer.IndexedAs(type, TmpIndexedTypes);
        foreach (var entry in TmpIndexedTypes)
        {
            result |= RemoveDefinition(definition, entry);
        }

        return result;
    }

    public void Clear()
    {
        var definitionsByType = _data.Values.ToList();
        foreach (var definitionMap in definitionsByType)
        {
            definitionMap.Clear();
        }

        _data.Clear();
    }

    #endregion Manipulation
    
    #region Queries

    public IEnumerable<Definition> All()
    {
        foreach (var outerPair in _data)
        {
            foreach (var innerPair in outerPair.Value)
            {
                yield return innerPair.Value;
            }
        }
    }

    /// <summary>
    /// Returns all definitions of a given type.
    /// </summary>
    /// <typeparam name="T">Definition type</typeparam>
    /// <returns>Definition of type, if found, null otherwise</returns>
    public IEnumerable<T> ByType<T>() where T : Definition
    {
        var collection = GetOrCreateCollection(typeof(T));
        if (collection is null)
        {
            yield break;
        }
        
        foreach (var definition in collection)
        {
            yield return (T) definition.Value;
        }
    }

    /// <summary>
    /// Returns a definition with the given id.
    /// </summary>
    /// <param name="id"></param>
    /// <typeparam name="T">Definition type</typeparam>
    /// <returns>Definition of type and id, if found, null otherwise</returns>
    public T? ById<T>(DefinitionId id) where T : Definition
    {
        Type type = typeof(T);
        var collection = GetOrCreateCollection(type);
        if (collection is null)
        {
            return null;
        }
        
        if (!collection.TryGetValue(id, out var result))
        {
            return null;
        }
        
        return (T) result;
    }

    /// <summary>
    /// Returns true if definition exists.
    /// </summary>
    /// <param name="id"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public bool Contains<T>(DefinitionId id) where T : Definition
    {
        Type type = typeof(T);
        return GetOrCreateCollection(type)?.ContainsKey(id) ?? false;
    }

    #endregion Queries
    
    #region Private

    private Dictionary<DefinitionId, Definition>? GetOrCreateCollection(Type collectionType, bool allowCreation = false)
    {
        if (!_data.TryGetValue(collectionType, out var innerDefinitions) && allowCreation)
        {
            return _data[collectionType] = new Dictionary<DefinitionId, Definition>();
        }

        return innerDefinitions;
    }
        
    private void AddDefinition(Definition definition, Type type)
    {
        GetOrCreateCollection(type, true)?.Add(definition.Id, definition);
    }

    private bool RemoveDefinition(Definition definition, Type type)
    {
        return GetOrCreateCollection(type)?.Remove(definition.Id) ?? false;
    }

    #endregion Private

    void IDisposable.Dispose()
    {
        _data.Clear();
    }
}
using RedHerring.Alexandria.Extensions.Collections;

namespace RedHerring.Clues;

public sealed class UnsortedDefinitionProcessor : DefinitionProcessor, IDisposable
{
    private List<SerializedDefinition> _serializedData = new();
    private DefinitionIndexer _indexer;
    
    #region Lifecycle

    public UnsortedDefinitionProcessor(DefinitionIndexer indexer)
    {
        _indexer = indexer;
    }

    public void Dispose()
    {
        _serializedData.Clear();
    }

    #endregion Lifecycle

    #region ILoadDefinition

    public void AddSerialized(SerializedDefinition definition) => _serializedData.Add(definition);
    public void AddSerialized(IEnumerable<SerializedDefinition> definitions) => _serializedData.AddRange(definitions);

    public void Process(DefinitionSet set)
    {
        if (_serializedData.IsNullOrEmpty())
        {
            return;
        }

        for (int i = 0; i < _serializedData.Count; i++)
        {
            var entry = _serializedData[i];
            var instance = Instantiate(entry.GetType());
            try
            {
                instance?.Init(entry);
            }
            catch (DefinitionException de)
            {
                // TODO: logging
                continue;
            }
            catch (Exception e)
            {
                // TODO: logging
                continue;
            }

            if (instance is not null)
            {
                set.Add(instance);
            }
        }
        
        _serializedData.Clear();
    }

    #endregion ILoadDefinition

    #region Private

    private Definition? Instantiate(Type serializedType)
    {
        var type = _indexer.DefinitionType(serializedType);
        if (type is null)
        {
            return null;
        }

        return Activator.CreateInstance(type) as Definition;
    }

    #endregion Private
}
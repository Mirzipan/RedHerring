using RedHerring.Alexandria.Extensions;
using RedHerring.Alexandria.Extensions.Collections;

namespace RedHerring.Clues;

public sealed class UnsortedDefinitionProcessor : DefinitionProcessor, IDisposable
{
    private List<SerializedDefinition> _serializedData = new();
    private DefinitionIndexer _indexer;
    private Action<DefinitionProcessor> _onDispose;
    
    #region Lifecycle

    internal UnsortedDefinitionProcessor(DefinitionIndexer indexer, Action<DefinitionProcessor> onDispose)
    {
        _indexer = indexer;
        _onDispose = onDispose;
    }

    public void Dispose()
    {
        _onDispose.SafeInvoke(this);
        
        _serializedData.Clear();
    }

    #endregion Lifecycle

    #region DefinitionProcessor

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

    #endregion DefinitionProcessor

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
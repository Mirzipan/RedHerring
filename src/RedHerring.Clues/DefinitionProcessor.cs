namespace RedHerring.Clues;

public interface DefinitionProcessor
{
    /// <summary>
    /// Adds a serialized definition to processing queue.
    /// </summary>
    /// <param name="definition"></param>
    public void AddSerialized(SerializedDefinition definition);
    
    /// <summary>
    /// Adds a serialized definitions to processing queue.
    /// </summary>
    /// <param name="definitions"></param>
    public void AddSerialized(IEnumerable<SerializedDefinition> definitions);
    
    /// <summary>
    /// Processes queue definitions and modifies the definition set accordingly.
    /// </summary>
    /// <param name="set"></param>
    public void Process(DefinitionSet set);
}
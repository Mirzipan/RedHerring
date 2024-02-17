namespace RedHerring.Clues;

public sealed class DefinitionsContext : IDisposable
{
    public readonly DefinitionSet Data;
    public readonly Dictionary<Type, Definition> Defaults = new();
    
    internal DefinitionsContext(DefinitionSet data)
    {
        Data = data;
        
        PopulateDefaults();
    }

    private void PopulateDefaults()
    {
        foreach (var entry in Data.All())
        {
            if (!entry.IsDefault)
            {
                continue;
            }

            var type = entry.GetType();
            Defaults[type] = entry;
        }
    }

    void IDisposable.Dispose()
    {
        Data.Dispose();
        Defaults.Clear();
    }
}
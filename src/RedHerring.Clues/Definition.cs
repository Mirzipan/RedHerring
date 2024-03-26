namespace RedHerring.Clues;

public abstract class Definition(DefinitionId id, string name, bool isDefault) : IDisposable
{
    public readonly DefinitionId Id = id;
    public readonly string Name = name;
    
    private bool _isDefault = isDefault;

    public bool IsDefault => _isDefault;

    #region Lifecycle

    public virtual void Dispose()
    {
    }

    internal void SetDefault(bool isDefault) => _isDefault = isDefault;

    #endregion Lifecycle
}
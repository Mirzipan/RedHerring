namespace RedHerring.Clues;

public abstract class Definition : IDisposable
{
    private Guid _id;
    private string _name;
    private bool _isDefault;

    public Guid Id => _id;
    public string Name => _name;
    public bool IsDefault => _isDefault;

    #region Lifecycle

    public Definition(Guid id, string name, bool isDefault)
    {
        _id = id;
        _name = name;
        _isDefault = isDefault;
    }

    public virtual void Dispose()
    {
    }

    internal void SetDefault(bool isDefault) => _isDefault = isDefault;

    #endregion Lifecycle
}
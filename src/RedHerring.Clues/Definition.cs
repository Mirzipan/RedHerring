namespace RedHerring.Clues;

public abstract class Definition : IDisposable
{
    private Guid _id;
    private bool _isDefault;

    public Guid Id => _id;
    public bool IsDefault => _isDefault;

    #region Lifecycle

    public Definition(Guid id, bool isDefault)
    {
        _id = id;
        _isDefault = isDefault;
    }

    public virtual void Dispose()
    {
    }

    internal void SetDefault(bool isDefault) => _isDefault = isDefault;

    #endregion Lifecycle
}
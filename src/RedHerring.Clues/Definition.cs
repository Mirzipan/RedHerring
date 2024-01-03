namespace RedHerring.Clues;

[DeserializedFrom(typeof(SerializedDefinition))]
public abstract class Definition : IDisposable
{
    private Guid _id;
    private bool _isDefault;

    public Guid Id => _id;
    public bool IsDefault => _isDefault;

    #region Lifecycle

    public virtual void Init(SerializedDefinition data)
    {
        _id = new Guid(data.Id);
        _isDefault = data.IsDefault;
    }

    public virtual void Dispose()
    {
    }

    internal void SetDefault(bool isDefault) => _isDefault = isDefault;

    #endregion Lifecycle
}
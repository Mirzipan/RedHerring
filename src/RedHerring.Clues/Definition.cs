
using RedHerring.Alexandria.Identifiers;

namespace RedHerring.Clues;

[DeserializedFrom(typeof(SerializedDefinition))]
public abstract class Definition : IDisposable
{
    private CompositeId _id;
    private bool _isDefault;

    public CompositeId Id => _id;
    public bool IsDefault => _isDefault;

    #region Lifecycle

    public virtual void Init(SerializedDefinition data)
    {
        _id = new CompositeId(data.PrimaryId, data.SecondaryId);
        _isDefault = data.IsDefault;
    }

    public virtual void Dispose()
    {
    }

    internal void SetDefault(bool isDefault) => _isDefault = isDefault;

    #endregion Lifecycle
}
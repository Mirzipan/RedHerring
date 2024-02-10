namespace RedHerring.Assets;
/*
public sealed record class AssetReference(AssetId Id)
{
    #region Lifecycle

    public static AssetReference New(AssetId id) => new(id);

    #endregion Lifecycle

    #region Conversion

    public override string ToString() => Id.ToString();

    #endregion Conversion
    
    #region Comparison

    public bool Equals(AssetReference? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Id.Equals(other.Id);
    }

    public override int GetHashCode() => Id.GetHashCode();

    #endregion Comparison
}
*/
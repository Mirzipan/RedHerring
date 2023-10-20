namespace RedHerring.Assets;

public readonly record struct AssetId : IComparable<AssetId>
{
    public static readonly AssetId Empty = new AssetId();
    
    private readonly Guid _guid;

    #region Lifecycle

    public AssetId(Guid guid)
    {
        _guid = guid;
    }

    public AssetId(string guid)
    {
        _guid = new Guid(guid);
    }

    public static AssetId New() => new(Guid.NewGuid());

    #endregion Lifecycle

    public override int GetHashCode() => _guid.GetHashCode();

    #region Conversion

    public static explicit operator AssetId(Guid guid) => new(guid);

    public static explicit operator Guid(AssetId id) => id._guid;

    #endregion Conversion

    #region Comparison

    public int CompareTo(AssetId other) => _guid.CompareTo(other._guid);

    public bool Equals(AssetId other) => _guid == other._guid;

    public override string ToString() => _guid.ToString();

    #endregion Comparison
}
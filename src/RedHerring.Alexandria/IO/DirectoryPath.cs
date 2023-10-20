namespace RedHerring.Alexandria.IO;

public sealed record class DirectoryPath : APath
{
    public static readonly DirectoryPath Empty = new(string.Empty);
    public static readonly DirectoryPath This = new(".");
    
    #region Lifecycle

    public DirectoryPath(string fullPath) : base(fullPath, true)
    {
    }

    #endregion Lifecycle

    public ReadOnlySpan<char> DirectoryName
    {
        get
        {
            int index = FullPath.LastIndexOf(Path.PathSeparator);
            return index >= 0 ? FullPath.AsSpan(index + 1) : null;
        }
    }

    #region Conversion

    public static implicit operator DirectoryPath?(string? fullPath)
    {
        return fullPath is not null ? new DirectoryPath(fullPath) : null;
    }

    #endregion Conversion
}
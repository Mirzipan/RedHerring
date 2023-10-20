using RedHerring.Alexandria.Extensions;
using RedHerring.Numbers;

namespace RedHerring.Alexandria.IO;

public abstract record class APath : IComparable
{
    private string _fullPath;
    
    protected readonly Region DriveRegion;
    protected readonly Region DirectoryRegion;
    protected readonly Region NameRegion;
    protected readonly Region ExtensionRegion;

    public string FullPath => _fullPath;

    public bool HasDrive => DriveRegion.IsValid;
    public bool HasDirectory => !IsFile && NameRegion.Start > 0;
    public bool IsFile => NameRegion.IsValid || ExtensionRegion.IsValid;

    #region Lifecycle

    protected APath(string fullPath, bool isDirectory)
    {
        // TODO: maybe some validation?
        
        _fullPath = Decode(fullPath, isDirectory, out DriveRegion, out DirectoryRegion, out NameRegion, out ExtensionRegion);
    }

    #endregion Lifecycle

    public ReadOnlySpan<char> Drive => FullPath.AsSpanIfValid(DriveRegion);

    #region Conversion

    public static implicit operator string?(APath? path) => path?.FullPath;

    public override string ToString() => _fullPath;

    #endregion Conversion

    #region Comparison

    public int CompareTo(object? obj)
    {
        if (obj is APath path)
        {
            return string.Compare(FullPath, path.FullPath, StringComparison.OrdinalIgnoreCase);
        }

        return 0;
    }

    #endregion Comparison

    #region Private

    private static string Decode(string filePath, bool isDirectory, out Region driveRegion, out Region directoryRegion,
        out Region nameRegion, out Region extensionRegion)
    {
        driveRegion = new Region();
        directoryRegion = new Region();
        nameRegion = new Region();
        extensionRegion = new Region();

        if (filePath.IsNullOrWhitespace())
        {
            return string.Empty;
        }

        // TODO: normalize the separators
        // TODO: split path into pieces and calculate regions
        
        return filePath;
    }

    #endregion Private
}
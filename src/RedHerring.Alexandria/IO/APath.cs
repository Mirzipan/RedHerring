using RedHerring.Alexandria.Extensions;
using RedHerring.Numbers;

namespace RedHerring.Alexandria.IO;

public abstract record class APath : IComparable
{
    private string _fullPath;
    
    // TODO: replace with ArraySegment<char>?
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

    private static string Decode(string filePath, bool isDirectory, out Region drive, out Region directory,
        out Region name, out Region extension)
    {
        drive = new Region();
        directory = new Region();
        name = new Region();
        extension = new Region();

        if (filePath.IsNullOrWhitespace())
        {
            return string.Empty;
        }

        // TODO: normalize the separators
        
        var root = Path.GetPathRoot(filePath.AsSpan());
        if (root.Length > 0)
        {
            drive.Length = root.Length;
        }
        
        var dir = Path.GetDirectoryName(filePath.AsSpan());
        var file = Path.GetFileNameWithoutExtension(filePath.AsSpan());
        
        if (drive.IsValid)
        {
            directory.Start = drive.Next;
            directory.Length = dir.Length;
        }
        else
        {
            directory.Start = 0;
            directory.Length = dir.Length;
        }

        if (file.Length > 0)
        {
            name.Start = directory.Next;
            name.Length = file.Length;
        }
        
        if (name.IsValid)
        {
            var ext = Path.GetExtension(filePath.AsSpan());
            extension.Length = ext.Length;
            extension.Start = filePath.Length - ext.Length;
        }
        
        return filePath;
    }

    #endregion Private
}
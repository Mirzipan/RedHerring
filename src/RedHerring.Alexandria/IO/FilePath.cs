using RedHerring.Alexandria.Extensions;
using RedHerring.Numbers;

namespace RedHerring.Alexandria.IO;

public record class FilePath : APath
{
    #region Lifecycle

    public FilePath(string fullPath) : base(fullPath, false)
    {
    }

    #endregion Lifecycle

    public ReadOnlySpan<char> FileName
    {
        get
        {
            var region = NameRegion;
            if (ExtensionRegion.IsValid)
            {
                region.Length = ExtensionRegion.Next - region.Start;
            }

            return FullPath.AsSpanIfValid(region);
        }
    }

    public ReadOnlySpan<char> BaseName => FullPath.AsSpanIfValid(NameRegion);

    public ReadOnlySpan<char> Extension => FullPath.AsSpanIfValid(ExtensionRegion);

    public ReadOnlySpan<char> Directory => FullPath.AsSpanIfValid(DirectoryRegion);

    public ReadOnlySpan<char> DirectoryAndFileName
    {
        get
        {
            var region = DirectoryRegion;
            if (ExtensionRegion.IsValid)
            {
                region.Length = ExtensionRegion.Next - region.Start;
            }
            else if (NameRegion.IsValid)
            {
                region.Length = NameRegion.Next - region.Start;
            }
            
            return FullPath.AsSpanIfValid(region);
        }
    }

    public ReadOnlySpan<char> DirectoryAndBaseName
    {
        get
        {
            var region = DirectoryRegion;
            if (NameRegion.IsValid)
            {
                region.Length = NameRegion.Next - region.Start;
            }

            return FullPath.AsSpanIfValid(region);
        }
    }

    public ReadOnlySpan<char> FullPathWithoutExtension
    {
        get
        {
            var region = new Region(0, FullPath.Length);
            if (NameRegion.IsValid)
            {
                region.Length = NameRegion.Next;
            }

            return FullPath.AsSpanIfValid(region);
        }
    }
}
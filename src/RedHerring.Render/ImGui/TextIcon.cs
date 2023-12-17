namespace RedHerring.Render.ImGui;

public static class TextIcon
{
    public const string ReorderList = FontAwesome6.Bars;
    
    #region Filesystem

    private static readonly List<string> ImageFiles = new()
    {
        ".png", ".jpg", ".jpeg", ".bmp", ".tiff", ".tif",
    };

    private static readonly List<string> SoundFiles = new()
    {
        ".wav", ".ogg",
    };
    
    public static string Folder(bool isEmpty)
    {
        return isEmpty ? FontAwesome6.FolderClosed : FontAwesome6.Folder;
    }

    public static string File(string name)
    {
        string extension = Path.GetExtension(name).ToLowerInvariant();
        if (ImageFiles.Contains(extension))
        {
            return FontAwesome6.FileImage;
        }

        if (SoundFiles.Contains(extension))
        {
            return FontAwesome6.FileAudio;
        }
        
        return extension switch
        {
            ".cs" => FontAwesome6.FileCode,
            ".csv" => FontAwesome6.FileCsv,
            ".pdf" => FontAwesome6.FilePdf,
            _ => FontAwesome6.File,
        };
    }

    #endregion Filesystem
}
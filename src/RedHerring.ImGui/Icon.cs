using IconFonts;
using RedHerring.Alexandria.Extensions;
using static ImGuiNET.ImGui;

namespace RedHerring.ImGui;

public static class Icon
{
    #region Filesystem

    private static readonly List<string> ImageFiles = new()
    {
        ".png", ".jpg", ".jpeg", ".bmp", ".tiff", ".tif",
    };

    private static readonly List<string> SoundFiles = new()
    {
        ".wav", ".ogg",
    };

    public static void Folder(bool isEmpty)
    {
        SetNextItemWidth(30);
        Text(FolderIconText(isEmpty));
        
        SameLine();
    }
    
    public static string FolderIconText(bool isEmpty)
    {
        return isEmpty ? FontAwesome6.FolderClosed : FontAwesome6.Folder;
    }

    public static void File(string name)
    {
        SetNextItemWidth(30);
        Text(FileIconText(name));
        
        SameLine();
    }

    public static string FileIconText(string name)
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

    #region List

    public static void ReorderList()
    {
        Button(FontAwesome6.Bars);
    }

    #endregion List
}
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
        PushFont(isEmpty ? Font.FARegular : Font.FASolid);
        SetNextItemWidth(30);
        Text(FontAwesome6.Folder);
        PopFont();
        
        SameLine();
    }

    public static void File(string name)
    {
        PushFont(Font.FASolid);
        SetNextItemWidth(30);
        Text(IconByExtension(name));
        PopFont();
        
        SameLine();
    }

    private static string IconByExtension(string name)
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
        PushFont(Font.FASolid);
        Text(FontAwesome6.Sort);
        PopFont();
    }

    #endregion List
}
using IconFonts;
using static ImGuiNET.ImGui;

namespace RedHerring.ImGui;

public static class Icon
{
    private static readonly List<string> ImageFiles = new()
    {
        ".png", ".jpg", ".jpeg",
    };
    private static readonly List<string> SoundFiles = new()
    {
        ".wav", ".ogg",
    };
    
    public static void Folder(bool isEmpty)
    {
        PushFont(Font.FASolid);
        SetNextItemWidth(30);
        Text(isEmpty ? FontAwesome6.FolderOpen : FontAwesome6.FolderClosed);
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
            ".pdf" => FontAwesome6.FilePdf,
            ".cs" => FontAwesome6.FileCode,
            _ => FontAwesome6.File,
        };
    }
}
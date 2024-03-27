using static ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface.Editor;

internal static class TextureFile
{
    private static readonly string[] Extensions =
    [
        ".png",
        ".jpg",
        ".jpeg",
        ".tga",
    ];
    
    public static bool HasExtension(string extension)
    {
        return Extensions.Contains(extension);
    }
    
    public static void Draw(IntPtr binding)
    {
        var size = GetContentRegionAvail();
        if (size.X > size.Y)
        {
            size.X = size.Y;
        }
        else if (size.X < size.Y)
        {
            size.Y = size.X;
        }
        
        Image(binding, size);
    }
}
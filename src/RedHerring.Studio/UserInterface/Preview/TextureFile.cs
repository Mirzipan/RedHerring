using System.Numerics;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface.Editor;

internal static class TextureFile
{
    private static readonly string[] ValidExtensions =
    [
        ".png",
        ".jpg",
        ".jpeg",
        ".tga",
    ];
    
    public static bool IsTexture(string extension)
    {
        return ValidExtensions.Contains(extension);
    }
    
    public static void Draw(IntPtr binding)
    {
        var size = Gui.GetContentRegionAvail();
        if (size.X > size.Y)
        {
            size.X = size.Y;
        }
        else if (size.X < size.Y)
        {
            size.Y = size.X;
        }
        
        Gui.Image(binding, size);
    }
}
using System.Numerics;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface.Editor;

internal static class TextureFile
{
    private static readonly string[] ValidExtensions =
    [
        ".png",
    ];
    
    public static bool IsTexture(string extension)
    {
        return ValidExtensions.Contains(extension);
    }
    
    public static void Draw(IntPtr binding)
    {
        var size = Gui.GetWindowSize() - new Vector2(40);
        var center = size / 2;
        if (size.X > size.Y)
        {
            size.X = size.Y;
        }
        else if (size.X < size.Y)
        {
            size.Y = size.X;
        }
        
        Gui.SetNextWindowPos(center);
        Gui.Image(binding, size);
    }
}
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
        Gui.Image(binding, new Vector2(400, 400));
    }
}
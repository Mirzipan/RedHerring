using System.Numerics;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface.Editor;

internal static class TextureFile
{
    public static void Draw(IntPtr binding)
    {
        Gui.Image(binding, new Vector2(400, 400));
    }
}
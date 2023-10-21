using ImGuiNET;
using Veldrid;

namespace RedHerring.ImGui;

internal static class FontLoader
{
    public static unsafe ImFontConfigPtr LoadFontData(string base64data, out byte[] fontData)
    {
        fontData = Convert.FromBase64String(base64data);

        ImFontConfig* imFontConfigPtr = ImGuiNative.ImFontConfig_ImFontConfig();
        imFontConfigPtr->FontDataOwnedByAtlas = 0;
        return new ImFontConfigPtr(imFontConfigPtr);
    }

    public static unsafe void RecreateFont(ImGuiRenderer renderer, byte[] fontData, ImFontConfigPtr configPtr)
    {
        fixed (byte* fontDataPtr = fontData)
        {
            ImGuiNET.ImGui.GetIO().Fonts.AddFontFromMemoryTTF((IntPtr)fontDataPtr, fontData.Length, 20, configPtr);
        }
        renderer.RecreateFontDeviceTexture();
    }

    public static void Unload()
    {
        ImGuiNET.ImGui.GetIO().Fonts.Clear();
    }

    public static unsafe void Unloaded(ImFontConfigPtr configPtr)
    {
        ImGuiNative.ImFontConfig_destroy(configPtr);
    }
}
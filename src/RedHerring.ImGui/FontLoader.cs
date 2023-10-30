using IconFonts;
using ImGuiNET;
using Veldrid;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.ImGui;

internal static class FontLoader
{
    public const string DefaultPath = "";
    public const string DefaultFontFile = "Roboto-Regular.ttf";
    public const int Size = 18;
    
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
            Gui.GetIO().Fonts.AddFontFromMemoryTTF((IntPtr)fontDataPtr, fontData.Length, 20, configPtr);
        }
        renderer.RecreateFontDeviceTexture();
    }

    public static void Unload()
    {
        Gui.GetIO().Fonts.Clear();

        Font.Default = null;
        Font.FARegular = null;
        Font.FASolid = null;
    }

    public static unsafe void Unloaded(ImFontConfigPtr configPtr)
    {
        ImGuiNative.ImFontConfig_destroy(configPtr);
    }

    public static void LoadDefaultFont()
    {
        Gui.GetIO().Fonts.AddFontDefault();
    }

    public static void LoadFonts(ImGuiRenderer renderer)
    {
        //Gui.GetIO().Fonts.AddFontDefault();
        Font.Default = Gui.GetIO().Fonts.AddFontFromFileTTF(Path.Combine(DefaultPath, DefaultFontFile), Size);
        Font.FARegular = Gui.GetIO().Fonts.AddFontFromFileTTF(Path.Combine(DefaultPath, FontAwesome6.FontIconFileNameFAR), Size);
        Font.FASolid = Gui.GetIO().Fonts.AddFontFromFileTTF(Path.Combine(DefaultPath, FontAwesome6.FontIconFileNameFAS), Size);
        renderer.RecreateFontDeviceTexture();
    }
}
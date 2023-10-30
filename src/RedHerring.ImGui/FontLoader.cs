using IconFonts;
using ImGuiNET;
using Veldrid;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.ImGui;

internal static class FontLoader
{
    public const string DefaultPath = "Resources";
    public const string DefaultFontFile = "Roboto-Regular.ttf";
    public const int Size = 16;

    private static readonly ushort[] GlyphRange = { FontAwesome6.IconMin, FontAwesome6.IconMax, 0 };
    private static readonly IntPtr GlyphRangePtr;

    static unsafe FontLoader()
    {
        fixed (ushort* ptr = GlyphRange)
        {
            GlyphRangePtr = (IntPtr)ptr;
        }
    }

    public static void Unload()
    {
        Gui.GetIO().Fonts.Clear();
        Font.Default = null;
        Font.FARegular = null;
        Font.FASolid = null;
    }

    public static void Load(ImGuiRenderer renderer)
    {
        Unload();
        LoadDefault();

        Font.FARegular = LoadIcons(FontAwesome6.FontIconFileNameFAR);
        Font.FASolid = LoadIcons(FontAwesome6.FontIconFileNameFAS);
        
        renderer.RecreateFontDeviceTexture();

        Gui.GetIO().Fonts.AddFontDefault(Font.Default.ConfigData);
    }

    private static void LoadDefault()
    {
        Font.Default = Gui.GetIO().Fonts.AddFontFromFileTTF(Path.Combine(DefaultPath, DefaultFontFile), Size);
        if (!Font.Default.IsLoaded())
        {
            Font.Default = Gui.GetIO().Fonts.AddFontDefault();
        }
    }

    private static ImFontPtr LoadIcons(string fileName)
    {
        string path = Path.Combine(DefaultPath, fileName);
        return Gui.GetIO().Fonts.AddFontFromFileTTF(path, Size, default, GlyphRangePtr);
    }
}
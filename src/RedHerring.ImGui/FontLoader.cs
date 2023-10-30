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

    private static readonly int[] GlyphRange = { FontAwesome6.IconMin, FontAwesome6.IconMax, 0 };
    private static readonly IntPtr GlyphRangePtr;

    private static readonly List<ImFontConfigPtr> Configs = new();

    static unsafe FontLoader()
    {
        fixed (int* ptr = GlyphRange)
        {
            GlyphRangePtr = (IntPtr)ptr;
        }
    }

    public static unsafe void Unload()
    {
        foreach (var config in Configs)
        {
            ImGuiNative.ImFontConfig_destroy(config);
        }
        
        Gui.GetIO().Fonts.Clear();

        Font.Default = null;
        Font.FARegular = null;
        Font.FASolid = null;
    }

    public static void LoadFonts(ImGuiRenderer renderer)
    {
        Font.Default = Gui.GetIO().Fonts.AddFontFromFileTTF(Path.Combine(DefaultPath, DefaultFontFile), Size);
        
        Font.FARegular = LoadFontAwesome(FontAwesome6.FontIconFileNameFAR);
        Font.FASolid = LoadFontAwesome(FontAwesome6.FontIconFileNameFAS);
        
        renderer.RecreateFontDeviceTexture();

        Gui.GetIO().Fonts.AddFontDefault(Font.Default.ConfigData);
    }
    
    // TODO: make work somehow
    private static unsafe ImFontPtr LoadFontAwesome(string fileName)
    {
        ImFontConfig* imFontConfigPtr = ImGuiNative.ImFontConfig_ImFontConfig();
        var config = new ImFontConfigPtr(imFontConfigPtr)
        {
            GlyphRanges = GlyphRangePtr,
            MergeMode = true,
            PixelSnapH = true,
        };
        Configs.Add(config);
        
        return Gui.GetIO().Fonts.AddFontFromFileTTF(Path.Combine(DefaultPath, fileName), Size, imFontConfigPtr, GlyphRangePtr);
    }
}
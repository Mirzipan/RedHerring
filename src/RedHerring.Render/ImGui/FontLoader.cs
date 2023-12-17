using ImGuiNET;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Render.ImGui;

internal static class FontLoader
{
    public const string DefaultPath = "Resources";
    public const string DefaultFontFile = "Roboto-Regular.ttf";
    public const int Size = 16;

    private static readonly ushort[] GlyphRange = { FontAwesome6.IconMin, FontAwesome6.IconMax, 0 };

    public static void Unload()
    {
        Gui.GetIO().Fonts.Clear();
    }

    public static void Load(ImGuiRenderer renderer)
    {
        Unload();
        LoadDefault();
        LoadIcons(FontAwesome6.FontIconFileNameFAS);
        LoadIcons(FontAwesome6.FontIconFileNameFAR);
        Gui.GetIO().Fonts.Build();
        
        renderer.RecreateFontDeviceTexture();
    }

    private static void LoadDefault()
    {
        try
        {
            Gui.GetIO().Fonts.AddFontFromFileTTF(Path.Combine(DefaultPath, DefaultFontFile), Size);
        }
        catch
        {
            Gui.GetIO().Fonts.AddFontDefault();
        }
    }

    private static unsafe void LoadIcons(string fileName)
    {
        string path = Path.Combine(DefaultPath, fileName);

        fixed (ushort* glyphRangePtr = GlyphRange)
        {
            ImFontConfig config = new()
                                  {
                                      GlyphRanges        = glyphRangePtr,
                                      MergeMode          = 1,
                                      OversampleH        = 1,
                                      OversampleV        = 1,
                                      PixelSnapH         = 1,
                                      RasterizerMultiply = 1,
                                      GlyphMinAdvanceX   = Size,
                                      GlyphMaxAdvanceX   = 256,
                                      
                                  };

            Gui.GetIO().Fonts.AddFontFromFileTTF(path, Size, &config);
        }
    }
}
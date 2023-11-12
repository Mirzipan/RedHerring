using System.Runtime.CompilerServices;

namespace RedHerring.Numbers;

public partial struct Color
{
    #region Named Colors

    /// <summary>
    /// Gets the color with RGBA value of #F0F8FFFF.
    /// </summary>
    public static Color AliceBlue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(240, 248, 255, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #FAEBD7FF.
    /// </summary>
    public static Color AntiqueWhite
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(250, 235, 215, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #00FFFFFF.
    /// </summary>
    public static Color Aqua
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(0, 255, 255, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #7FFFD4FF.
    /// </summary>
    public static Color Aquamarine
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(127, 255, 212, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #F0FFFFFF.
    /// </summary>
    public static Color Azure
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(240, 255, 255, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #F5F5DCFF.
    /// </summary>
    public static Color Beige
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(245, 245, 220, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #FFE4C4FF.
    /// </summary>
    public static Color Bisque
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(255, 228, 196, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #000000FF.
    /// </summary>
    public static Color Black
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(0, 0, 0, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #FFEBCDFF.
    /// </summary>
    public static Color BlanchedAlmond
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(255, 235, 205, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #0000FFFF.
    /// </summary>
    public static Color Blue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(0, 0, 255, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #8A2BE2FF.
    /// </summary>
    public static Color BlueViolet
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(138, 43, 226, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #A52A2AFF.
    /// </summary>
    public static Color Brown
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(165, 42, 42, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #DEB887FF.
    /// </summary>
    public static Color BurlyWood
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(222, 184, 135, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #5F9EA0FF.
    /// </summary>
    public static Color CadetBlue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(95, 158, 160, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #7FFF00FF.
    /// </summary>
    public static Color Chartreuse
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(127, 255, 0, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #D2691EFF.
    /// </summary>
    public static Color Chocolate
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(210, 105, 30, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #FF7F50FF.
    /// </summary>
    public static Color Coral
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(255, 127, 80, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #6495EDFF.
    /// </summary>
    public static Color CornflowerBlue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(100, 149, 237, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #FFF8DCFF.
    /// </summary>
    public static Color Cornsilk
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(255, 248, 220, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #DC143CFF.
    /// </summary>
    public static Color Crimson
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(220, 20, 60, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #00FFFFFF.
    /// </summary>
    public static Color Cyan
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(0, 255, 255, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #00008BFF.
    /// </summary>
    public static Color DarkBlue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(0, 0, 139, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #008B8BFF.
    /// </summary>
    public static Color DarkCyan
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(0, 139, 139, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #B8860BFF.
    /// </summary>
    public static Color DarkGoldenrod
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(184, 134, 11, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #A9A9A9FF.
    /// </summary>
    public static Color DarkGray
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(169, 169, 169, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #006400FF.
    /// </summary>
    public static Color DarkGreen
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(0, 100, 0, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #BDB76BFF.
    /// </summary>
    public static Color DarkKhaki
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(189, 183, 107, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #8B008BFF.
    /// </summary>
    public static Color DarkMagenta
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(139, 0, 139, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #556B2FFF.
    /// </summary>
    public static Color DarkOliveGreen
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(85, 107, 47, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #FF8C00FF.
    /// </summary>
    public static Color DarkOrange
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(255, 140, 0, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #9932CCFF.
    /// </summary>
    public static Color DarkOrchid
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(153, 50, 204, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #8B0000FF.
    /// </summary>
    public static Color DarkRed
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(139, 0, 0, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #E9967AFF.
    /// </summary>
    public static Color DarkSalmon
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(233, 150, 122, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #8FBC8FFF.
    /// </summary>
    public static Color DarkSeaGreen
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(143, 188, 143, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #483D8BFF.
    /// </summary>
    public static Color DarkSlateBlue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(72, 61, 139, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #2F4F4FFF.
    /// </summary>
    public static Color DarkSlateGray
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(47, 79, 79, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #00CED1FF.
    /// </summary>
    public static Color DarkTurquoise
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(0, 206, 209, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #9400D3FF.
    /// </summary>
    public static Color DarkViolet
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(148, 0, 211, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #FF1493FF.
    /// </summary>
    public static Color DeepPink
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(255, 20, 147, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #00BFFFFF.
    /// </summary>
    public static Color DeepSkyBlue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(0, 191, 255, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #696969FF.
    /// </summary>
    public static Color DimGray
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(105, 105, 105, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #1E90FFFF.
    /// </summary>
    public static Color DodgerBlue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(30, 144, 255, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #B22222FF.
    /// </summary>
    public static Color Firebrick
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(178, 34, 34, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #FFFAF0FF.
    /// </summary>
    public static Color FloralWhite
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(255, 250, 240, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #228B22FF.
    /// </summary>
    public static Color ForestGreen
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(34, 139, 34, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #FF00FFFF.
    /// </summary>
    public static Color Fuchsia
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(255, 0, 255, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #DCDCDCFF.
    /// </summary>
    public static Color Gainsboro
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(220, 220, 220, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #F8F8FFFF.
    /// </summary>
    public static Color GhostWhite
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(248, 248, 255, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #FFD700FF.
    /// </summary>
    public static Color Gold
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(255, 215, 0, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #DAA520FF.
    /// </summary>
    public static Color Goldenrod
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(218, 165, 32, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #808080FF.
    /// </summary>
    public static Color Gray
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(128, 128, 128, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #008000FF.
    /// </summary>
    public static Color Green
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(0, 128, 0, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #ADFF2FFF.
    /// </summary>
    public static Color GreenYellow
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(173, 255, 47, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #F0FFF0FF.
    /// </summary>
    public static Color Honeydew
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(240, 255, 240, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #FF69B4FF.
    /// </summary>
    public static Color HotPink
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(255, 105, 180, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #CD5C5CFF.
    /// </summary>
    public static Color IndianRed
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(205, 92, 92, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #4B0082FF.
    /// </summary>
    public static Color Indigo
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(75, 0, 130, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #FFFFF0FF.
    /// </summary>
    public static Color Ivory
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(255, 255, 240, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #F0E68CFF.
    /// </summary>
    public static Color Khaki
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(240, 230, 140, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #E6E6FAFF.
    /// </summary>
    public static Color Lavender
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(230, 230, 250, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #FFF0F5FF.
    /// </summary>
    public static Color LavenderBlush
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(255, 240, 245, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #7CFC00FF.
    /// </summary>
    public static Color LawnGreen
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(124, 252, 0, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #FFFACDFF.
    /// </summary>
    public static Color LemonChiffon
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(255, 250, 205, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #ADD8E6FF.
    /// </summary>
    public static Color LightBlue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(173, 216, 230, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #F08080FF.
    /// </summary>
    public static Color LightCoral
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(240, 128, 128, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #E0FFFFFF.
    /// </summary>
    public static Color LightCyan
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(224, 255, 255, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #FAFAD2FF.
    /// </summary>
    public static Color LightGoldenrodYellow
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(250, 250, 210, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #D3D3D3FF.
    /// </summary>
    public static Color LightGray
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(211, 211, 211, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #90EE90FF.
    /// </summary>
    public static Color LightGreen
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(144, 238, 144, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #FFB6C1FF.
    /// </summary>
    public static Color LightPink
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(255, 182, 193, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #FFA07AFF.
    /// </summary>
    public static Color LightSalmon
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(255, 160, 122, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #20B2AAFF.
    /// </summary>
    public static Color LightSeaGreen
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(32, 178, 170, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #87CEFAFF.
    /// </summary>
    public static Color LightSkyBlue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(135, 206, 250, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #778899FF.
    /// </summary>
    public static Color LightSlateGray
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(119, 136, 153, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #B0C4DEFF.
    /// </summary>
    public static Color LightSteelBlue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(176, 196, 222, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #FFFFE0FF.
    /// </summary>
    public static Color LightYellow
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(255, 255, 224, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #00FF00FF.
    /// </summary>
    public static Color Lime
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(0, 255, 0, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #32CD32FF.
    /// </summary>
    public static Color LimeGreen
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(50, 205, 50, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #FAF0E6FF.
    /// </summary>
    public static Color Linen
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(250, 240, 230, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #FF00FFFF.
    /// </summary>
    public static Color Magenta
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(255, 0, 255, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #800000FF.
    /// </summary>
    public static Color Maroon
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(128, 0, 0, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #66CDAAFF.
    /// </summary>
    public static Color MediumAquamarine
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(102, 205, 170, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #0000CDFF.
    /// </summary>
    public static Color MediumBlue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(0, 0, 205, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #BA55D3FF.
    /// </summary>
    public static Color MediumOrchid
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(186, 85, 211, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #9370DBFF.
    /// </summary>
    public static Color MediumPurple
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(147, 112, 219, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #3CB371FF.
    /// </summary>
    public static Color MediumSeaGreen
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(60, 179, 113, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #7B68EEFF.
    /// </summary>
    public static Color MediumSlateBlue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(123, 104, 238, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #00FA9AFF.
    /// </summary>
    public static Color MediumSpringGreen
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(0, 250, 154, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #48D1CCFF.
    /// </summary>
    public static Color MediumTurquoise
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(72, 209, 204, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #C71585FF.
    /// </summary>
    public static Color MediumVioletRed
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(199, 21, 133, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #191970FF.
    /// </summary>
    public static Color MidnightBlue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(25, 25, 112, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #F5FFFAFF.
    /// </summary>
    public static Color MintCream
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(245, 255, 250, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #FFE4E1FF.
    /// </summary>
    public static Color MistyRose
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(255, 228, 225, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #FFE4B5FF.
    /// </summary>
    public static Color Moccasin
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(255, 228, 181, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #FFDEADFF.
    /// </summary>
    public static Color NavajoWhite
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(255, 222, 173, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #000080FF.
    /// </summary>
    public static Color Navy
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(0, 0, 128, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #FDF5E6FF.
    /// </summary>
    public static Color OldLace
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(253, 245, 230, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #808000FF.
    /// </summary>
    public static Color Olive
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(128, 128, 0, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #6B8E23FF.
    /// </summary>
    public static Color OliveDrab
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(107, 142, 35, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #FFA500FF.
    /// </summary>
    public static Color Orange
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(255, 165, 0, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #FF4500FF.
    /// </summary>
    public static Color OrangeRed
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(255, 69, 0, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #DA70D6FF.
    /// </summary>
    public static Color Orchid
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(218, 112, 214, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #EEE8AAFF.
    /// </summary>
    public static Color PaleGoldenrod
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(238, 232, 170, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #98FB98FF.
    /// </summary>
    public static Color PaleGreen
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(152, 251, 152, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #AFEEEEFF.
    /// </summary>
    public static Color PaleTurquoise
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(175, 238, 238, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #DB7093FF.
    /// </summary>
    public static Color PaleVioletRed
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(219, 112, 147, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #FFEFD5FF.
    /// </summary>
    public static Color PapayaWhip
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(255, 239, 213, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #FFDAB9FF.
    /// </summary>
    public static Color PeachPuff
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(255, 218, 185, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #CD853FFF.
    /// </summary>
    public static Color Peru
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(205, 133, 63, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #FFC0CBFF.
    /// </summary>
    public static Color Pink
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(255, 192, 203, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #DDA0DDFF.
    /// </summary>
    public static Color Plum
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(221, 160, 221, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #B0E0E6FF.
    /// </summary>
    public static Color PowderBlue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(176, 224, 230, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #800080FF.
    /// </summary>
    public static Color Purple
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(128, 0, 128, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #FF0000FF.
    /// </summary>
    public static Color Red
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(255, 0, 0, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #BC8F8FFF.
    /// </summary>
    public static Color RosyBrown
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(188, 143, 143, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #4169E1FF.
    /// </summary>
    public static Color RoyalBlue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(65, 105, 225, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #8B4513FF.
    /// </summary>
    public static Color SaddleBrown
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(139, 69, 19, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #FA8072FF.
    /// </summary>
    public static Color Salmon
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(250, 128, 114, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #F4A460FF.
    /// </summary>
    public static Color SandyBrown
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(244, 164, 96, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #2E8B57FF.
    /// </summary>
    public static Color SeaGreen
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(46, 139, 87, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #FFF5EEFF.
    /// </summary>
    public static Color SeaShell
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(255, 245, 238, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #A0522DFF.
    /// </summary>
    public static Color Sienna
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(160, 82, 45, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #C0C0C0FF.
    /// </summary>
    public static Color Silver
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(192, 192, 192, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #87CEEBFF.
    /// </summary>
    public static Color SkyBlue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(135, 206, 235, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #6A5ACDFF.
    /// </summary>
    public static Color SlateBlue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(106, 90, 205, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #708090FF.
    /// </summary>
    public static Color SlateGray
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(112, 128, 144, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #FFFAFAFF.
    /// </summary>
    public static Color Snow
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(255, 250, 250, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #00FF7FFF.
    /// </summary>
    public static Color SpringGreen
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(0, 255, 127, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #4682B4FF.
    /// </summary>
    public static Color SteelBlue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(70, 130, 180, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #D2B48CFF.
    /// </summary>
    public static Color Tan
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(210, 180, 140, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #008080FF.
    /// </summary>
    public static Color Teal
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(0, 128, 128, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #D8BFD8FF.
    /// </summary>
    public static Color Thistle
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(216, 191, 216, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #FF6347FF.
    /// </summary>
    public static Color Tomato
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(255, 99, 71, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #FFFFFF00.
    /// </summary>
    public static Color Transparent
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(255, 255, 255, 0);
    }
    /// <summary>
    /// Gets the color with RGBA value of #40E0D0FF.
    /// </summary>
    public static Color Turquoise
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(64, 224, 208, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #EE82EEFF.
    /// </summary>
    public static Color Violet
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(238, 130, 238, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #F5DEB3FF.
    /// </summary>
    public static Color Wheat
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(245, 222, 179, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #FFFFFFFF.
    /// </summary>
    public static Color White
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(255, 255, 255, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #F5F5F5FF.
    /// </summary>
    public static Color WhiteSmoke
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(245, 245, 245, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #FFFF00FF.
    /// </summary>
    public static Color Yellow
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(255, 255, 0, 255);
    }
    /// <summary>
    /// Gets the color with RGBA value of #9ACD32FF.
    /// </summary>
    public static Color YellowGreen
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Color(154, 205, 50, 255);
    }

    #endregion Named Colors
}
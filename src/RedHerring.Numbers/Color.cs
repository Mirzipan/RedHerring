using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RedHerring.Numbers;

[Serializable]
[StructLayout(LayoutKind.Explicit)]
public struct Color : IEquatable<Color>
{
    internal const int Count = 4;

    [FieldOffset(0)]
    private readonly uint _value;

    [NonSerialized]
    [FieldOffset(0)]
    public readonly byte R;

    [NonSerialized]
    [FieldOffset(1)]
    public readonly byte G;

    [NonSerialized]
    [FieldOffset(2)]
    public readonly byte B;

    [NonSerialized]
    [FieldOffset(3)]
    public readonly byte A;

    public uint Value => _value;

    public byte this[int index]
    {
        get => GetElement(this, index);
        set => this = WithElement(this, index, value);
    }

    #region Named Colors

    /// <summary>
    /// Gets the color with RGBA value of #F0F8FFFF.
    /// </summary>
    public static readonly Color AliceBlue = new Color(240, 248, 255, 255);

    /// <summary>
    /// Gets the color with RGBA value of #FAEBD7FF.
    /// </summary>
    public static readonly Color AntiqueWhite = new Color(250, 235, 215, 255);

    /// <summary>
    /// Gets the color with RGBA value of #00FFFFFF.
    /// </summary>
    public static readonly Color Aqua = new Color(0, 255, 255, 255);

    /// <summary>
    /// Gets the color with RGBA value of #7FFFD4FF.
    /// </summary>
    public static readonly Color Aquamarine = new Color(127, 255, 212, 255);

    /// <summary>
    /// Gets the color with RGBA value of #F0FFFFFF.
    /// </summary>
    public static readonly Color Azure = new Color(240, 255, 255, 255);

    /// <summary>
    /// Gets the color with RGBA value of #F5F5DCFF.
    /// </summary>
    public static readonly Color Beige = new Color(245, 245, 220, 255);

    /// <summary>
    /// Gets the color with RGBA value of #FFE4C4FF.
    /// </summary>
    public static readonly Color Bisque = new Color(255, 228, 196, 255);

    /// <summary>
    /// Gets the color with RGBA value of #000000FF.
    /// </summary>
    public static readonly Color Black = new Color(0, 0, 0, 255);

    /// <summary>
    /// Gets the color with RGBA value of #FFEBCDFF.
    /// </summary>
    public static readonly Color BlanchedAlmond = new Color(255, 235, 205, 255);

    /// <summary>
    /// Gets the color with RGBA value of #0000FFFF.
    /// </summary>
    public static readonly Color Blue = new Color(0, 0, 255, 255);

    /// <summary>
    /// Gets the color with RGBA value of #8A2BE2FF.
    /// </summary>
    public static readonly Color BlueViolet = new Color(138, 43, 226, 255);

    /// <summary>
    /// Gets the color with RGBA value of #A52A2AFF.
    /// </summary>
    public static readonly Color Brown = new Color(165, 42, 42, 255);

    /// <summary>
    /// Gets the color with RGBA value of #DEB887FF.
    /// </summary>
    public static readonly Color BurlyWood = new Color(222, 184, 135, 255);

    /// <summary>
    /// Gets the color with RGBA value of #5F9EA0FF.
    /// </summary>
    public static readonly Color CadetBlue = new Color(95, 158, 160, 255);

    /// <summary>
    /// Gets the color with RGBA value of #7FFF00FF.
    /// </summary>
    public static readonly Color Chartreuse = new Color(127, 255, 0, 255);

    /// <summary>
    /// Gets the color with RGBA value of #D2691EFF.
    /// </summary>
    public static readonly Color Chocolate = new Color(210, 105, 30, 255);

    /// <summary>
    /// Gets the color with RGBA value of #FF7F50FF.
    /// </summary>
    public static readonly Color Coral = new Color(255, 127, 80, 255);

    /// <summary>
    /// Gets the color with RGBA value of #6495EDFF.
    /// </summary>
    public static readonly Color CornflowerBlue = new Color(100, 149, 237, 255);

    /// <summary>
    /// Gets the color with RGBA value of #FFF8DCFF.
    /// </summary>
    public static readonly Color Cornsilk = new Color(255, 248, 220, 255);

    /// <summary>
    /// Gets the color with RGBA value of #DC143CFF.
    /// </summary>
    public static readonly Color Crimson = new Color(220, 20, 60, 255);

    /// <summary>
    /// Gets the color with RGBA value of #00FFFFFF.
    /// </summary>
    public static readonly Color Cyan = new Color(0, 255, 255, 255);

    /// <summary>
    /// Gets the color with RGBA value of #00008BFF.
    /// </summary>
    public static readonly Color DarkBlue = new Color(0, 0, 139, 255);

    /// <summary>
    /// Gets the color with RGBA value of #008B8BFF.
    /// </summary>
    public static readonly Color DarkCyan = new Color(0, 139, 139, 255);

    /// <summary>
    /// Gets the color with RGBA value of #B8860BFF.
    /// </summary>
    public static readonly Color DarkGoldenrod = new Color(184, 134, 11, 255);

    /// <summary>
    /// Gets the color with RGBA value of #A9A9A9FF.
    /// </summary>
    public static readonly Color DarkGray = new Color(169, 169, 169, 255);

    /// <summary>
    /// Gets the color with RGBA value of #006400FF.
    /// </summary>
    public static readonly Color DarkGreen = new Color(0, 100, 0, 255);

    /// <summary>
    /// Gets the color with RGBA value of #BDB76BFF.
    /// </summary>
    public static readonly Color DarkKhaki = new Color(189, 183, 107, 255);

    /// <summary>
    /// Gets the color with RGBA value of #8B008BFF.
    /// </summary>
    public static readonly Color DarkMagenta = new Color(139, 0, 139, 255);

    /// <summary>
    /// Gets the color with RGBA value of #556B2FFF.
    /// </summary>
    public static readonly Color DarkOliveGreen = new Color(85, 107, 47, 255);

    /// <summary>
    /// Gets the color with RGBA value of #FF8C00FF.
    /// </summary>
    public static readonly Color DarkOrange = new Color(255, 140, 0, 255);

    /// <summary>
    /// Gets the color with RGBA value of #9932CCFF.
    /// </summary>
    public static readonly Color DarkOrchid = new Color(153, 50, 204, 255);

    /// <summary>
    /// Gets the color with RGBA value of #8B0000FF.
    /// </summary>
    public static readonly Color DarkRed = new Color(139, 0, 0, 255);

    /// <summary>
    /// Gets the color with RGBA value of #E9967AFF.
    /// </summary>
    public static readonly Color DarkSalmon = new Color(233, 150, 122, 255);

    /// <summary>
    /// Gets the color with RGBA value of #8FBC8FFF.
    /// </summary>
    public static readonly Color DarkSeaGreen = new Color(143, 188, 143, 255);

    /// <summary>
    /// Gets the color with RGBA value of #483D8BFF.
    /// </summary>
    public static readonly Color DarkSlateBlue = new Color(72, 61, 139, 255);

    /// <summary>
    /// Gets the color with RGBA value of #2F4F4FFF.
    /// </summary>
    public static readonly Color DarkSlateGray = new Color(47, 79, 79, 255);

    /// <summary>
    /// Gets the color with RGBA value of #00CED1FF.
    /// </summary>
    public static readonly Color DarkTurquoise = new Color(0, 206, 209, 255);

    /// <summary>
    /// Gets the color with RGBA value of #9400D3FF.
    /// </summary>
    public static readonly Color DarkViolet = new Color(148, 0, 211, 255);

    /// <summary>
    /// Gets the color with RGBA value of #FF1493FF.
    /// </summary>
    public static readonly Color DeepPink = new Color(255, 20, 147, 255);

    /// <summary>
    /// Gets the color with RGBA value of #00BFFFFF.
    /// </summary>
    public static readonly Color DeepSkyBlue = new Color(0, 191, 255, 255);

    /// <summary>
    /// Gets the color with RGBA value of #696969FF.
    /// </summary>
    public static readonly Color DimGray = new Color(105, 105, 105, 255);

    /// <summary>
    /// Gets the color with RGBA value of #1E90FFFF.
    /// </summary>
    public static readonly Color DodgerBlue = new Color(30, 144, 255, 255);

    /// <summary>
    /// Gets the color with RGBA value of #B22222FF.
    /// </summary>
    public static readonly Color Firebrick = new Color(178, 34, 34, 255);

    /// <summary>
    /// Gets the color with RGBA value of #FFFAF0FF.
    /// </summary>
    public static readonly Color FloralWhite = new Color(255, 250, 240, 255);

    /// <summary>
    /// Gets the color with RGBA value of #228B22FF.
    /// </summary>
    public static readonly Color ForestGreen = new Color(34, 139, 34, 255);

    /// <summary>
    /// Gets the color with RGBA value of #FF00FFFF.
    /// </summary>
    public static readonly Color Fuchsia = new Color(255, 0, 255, 255);

    /// <summary>
    /// Gets the color with RGBA value of #DCDCDCFF.
    /// </summary>
    public static readonly Color Gainsboro = new Color(220, 220, 220, 255);

    /// <summary>
    /// Gets the color with RGBA value of #F8F8FFFF.
    /// </summary>
    public static readonly Color GhostWhite = new Color(248, 248, 255, 255);

    /// <summary>
    /// Gets the color with RGBA value of #FFD700FF.
    /// </summary>
    public static readonly Color Gold = new Color(255, 215, 0, 255);

    /// <summary>
    /// Gets the color with RGBA value of #DAA520FF.
    /// </summary>
    public static readonly Color Goldenrod = new Color(218, 165, 32, 255);

    /// <summary>
    /// Gets the color with RGBA value of #808080FF.
    /// </summary>
    public static readonly Color Gray = new Color(128, 128, 128, 255);

    /// <summary>
    /// Gets the color with RGBA value of #008000FF.
    /// </summary>
    public static readonly Color Green = new Color(0, 128, 0, 255);

    /// <summary>
    /// Gets the color with RGBA value of #ADFF2FFF.
    /// </summary>
    public static readonly Color GreenYellow = new Color(173, 255, 47, 255);

    /// <summary>
    /// Gets the color with RGBA value of #F0FFF0FF.
    /// </summary>
    public static readonly Color Honeydew = new Color(240, 255, 240, 255);

    /// <summary>
    /// Gets the color with RGBA value of #FF69B4FF.
    /// </summary>
    public static readonly Color HotPink = new Color(255, 105, 180, 255);

    /// <summary>
    /// Gets the color with RGBA value of #CD5C5CFF.
    /// </summary>
    public static readonly Color IndianRed = new Color(205, 92, 92, 255);

    /// <summary>
    /// Gets the color with RGBA value of #4B0082FF.
    /// </summary>
    public static readonly Color Indigo = new Color(75, 0, 130, 255);

    /// <summary>
    /// Gets the color with RGBA value of #FFFFF0FF.
    /// </summary>
    public static readonly Color Ivory = new Color(255, 255, 240, 255);

    /// <summary>
    /// Gets the color with RGBA value of #F0E68CFF.
    /// </summary>
    public static readonly Color Khaki = new Color(240, 230, 140, 255);

    /// <summary>
    /// Gets the color with RGBA value of #E6E6FAFF.
    /// </summary>
    public static readonly Color Lavender = new Color(230, 230, 250, 255);

    /// <summary>
    /// Gets the color with RGBA value of #FFF0F5FF.
    /// </summary>
    public static readonly Color LavenderBlush = new Color(255, 240, 245, 255);

    /// <summary>
    /// Gets the color with RGBA value of #7CFC00FF.
    /// </summary>
    public static readonly Color LawnGreen = new Color(124, 252, 0, 255);

    /// <summary>
    /// Gets the color with RGBA value of #FFFACDFF.
    /// </summary>
    public static readonly Color LemonChiffon = new Color(255, 250, 205, 255);

    /// <summary>
    /// Gets the color with RGBA value of #ADD8E6FF.
    /// </summary>
    public static readonly Color LightBlue = new Color(173, 216, 230, 255);

    /// <summary>
    /// Gets the color with RGBA value of #F08080FF.
    /// </summary>
    public static readonly Color LightCoral = new Color(240, 128, 128, 255);

    /// <summary>
    /// Gets the color with RGBA value of #E0FFFFFF.
    /// </summary>
    public static readonly Color LightCyan = new Color(224, 255, 255, 255);

    /// <summary>
    /// Gets the color with RGBA value of #FAFAD2FF.
    /// </summary>
    public static readonly Color LightGoldenrodYellow = new Color(250, 250, 210, 255);

    /// <summary>
    /// Gets the color with RGBA value of #D3D3D3FF.
    /// </summary>
    public static readonly Color LightGray = new Color(211, 211, 211, 255);

    /// <summary>
    /// Gets the color with RGBA value of #90EE90FF.
    /// </summary>
    public static readonly Color LightGreen = new Color(144, 238, 144, 255);

    /// <summary>
    /// Gets the color with RGBA value of #FFB6C1FF.
    /// </summary>
    public static readonly Color LightPink = new Color(255, 182, 193, 255);

    /// <summary>
    /// Gets the color with RGBA value of #FFA07AFF.
    /// </summary>
    public static readonly Color LightSalmon = new Color(255, 160, 122, 255);

    /// <summary>
    /// Gets the color with RGBA value of #20B2AAFF.
    /// </summary>
    public static readonly Color LightSeaGreen = new Color(32, 178, 170, 255);

    /// <summary>
    /// Gets the color with RGBA value of #87CEFAFF.
    /// </summary>
    public static readonly Color LightSkyBlue = new Color(135, 206, 250, 255);

    /// <summary>
    /// Gets the color with RGBA value of #778899FF.
    /// </summary>
    public static readonly Color LightSlateGray = new Color(119, 136, 153, 255);

    /// <summary>
    /// Gets the color with RGBA value of #B0C4DEFF.
    /// </summary>
    public static readonly Color LightSteelBlue = new Color(176, 196, 222, 255);

    /// <summary>
    /// Gets the color with RGBA value of #FFFFE0FF.
    /// </summary>
    public static readonly Color LightYellow = new Color(255, 255, 224, 255);

    /// <summary>
    /// Gets the color with RGBA value of #00FF00FF.
    /// </summary>
    public static readonly Color Lime = new Color(0, 255, 0, 255);

    /// <summary>
    /// Gets the color with RGBA value of #32CD32FF.
    /// </summary>
    public static readonly Color LimeGreen = new Color(50, 205, 50, 255);

    /// <summary>
    /// Gets the color with RGBA value of #FAF0E6FF.
    /// </summary>
    public static readonly Color Linen = new Color(250, 240, 230, 255);

    /// <summary>
    /// Gets the color with RGBA value of #FF00FFFF.
    /// </summary>
    public static readonly Color Magenta = new Color(255, 0, 255, 255);

    /// <summary>
    /// Gets the color with RGBA value of #800000FF.
    /// </summary>
    public static readonly Color Maroon = new Color(128, 0, 0, 255);

    /// <summary>
    /// Gets the color with RGBA value of #66CDAAFF.
    /// </summary>
    public static readonly Color MediumAquamarine = new Color(102, 205, 170, 255);

    /// <summary>
    /// Gets the color with RGBA value of #0000CDFF.
    /// </summary>
    public static readonly Color MediumBlue = new Color(0, 0, 205, 255);

    /// <summary>
    /// Gets the color with RGBA value of #BA55D3FF.
    /// </summary>
    public static readonly Color MediumOrchid = new Color(186, 85, 211, 255);

    /// <summary>
    /// Gets the color with RGBA value of #9370DBFF.
    /// </summary>
    public static readonly Color MediumPurple = new Color(147, 112, 219, 255);

    /// <summary>
    /// Gets the color with RGBA value of #3CB371FF.
    /// </summary>
    public static readonly Color MediumSeaGreen = new Color(60, 179, 113, 255);

    /// <summary>
    /// Gets the color with RGBA value of #7B68EEFF.
    /// </summary>
    public static readonly Color MediumSlateBlue = new Color(123, 104, 238, 255);

    /// <summary>
    /// Gets the color with RGBA value of #00FA9AFF.
    /// </summary>
    public static readonly Color MediumSpringGreen = new Color(0, 250, 154, 255);

    /// <summary>
    /// Gets the color with RGBA value of #48D1CCFF.
    /// </summary>
    public static readonly Color MediumTurquoise = new Color(72, 209, 204, 255);

    /// <summary>
    /// Gets the color with RGBA value of #C71585FF.
    /// </summary>
    public static readonly Color MediumVioletRed = new Color(199, 21, 133, 255);

    /// <summary>
    /// Gets the color with RGBA value of #191970FF.
    /// </summary>
    public static readonly Color MidnightBlue = new Color(25, 25, 112, 255);

    /// <summary>
    /// Gets the color with RGBA value of #F5FFFAFF.
    /// </summary>
    public static readonly Color MintCream = new Color(245, 255, 250, 255);

    /// <summary>
    /// Gets the color with RGBA value of #FFE4E1FF.
    /// </summary>
    public static readonly Color MistyRose = new Color(255, 228, 225, 255);

    /// <summary>
    /// Gets the color with RGBA value of #FFE4B5FF.
    /// </summary>
    public static readonly Color Moccasin = new Color(255, 228, 181, 255);

    /// <summary>
    /// Gets the color with RGBA value of #FFDEADFF.
    /// </summary>
    public static readonly Color NavajoWhite = new Color(255, 222, 173, 255);

    /// <summary>
    /// Gets the color with RGBA value of #000080FF.
    /// </summary>
    public static readonly Color Navy = new Color(0, 0, 128, 255);

    /// <summary>
    /// Gets the color with RGBA value of #FDF5E6FF.
    /// </summary>
    public static readonly Color OldLace = new Color(253, 245, 230, 255);

    /// <summary>
    /// Gets the color with RGBA value of #808000FF.
    /// </summary>
    public static readonly Color Olive = new Color(128, 128, 0, 255);

    /// <summary>
    /// Gets the color with RGBA value of #6B8E23FF.
    /// </summary>
    public static readonly Color OliveDrab = new Color(107, 142, 35, 255);

    /// <summary>
    /// Gets the color with RGBA value of #FFA500FF.
    /// </summary>
    public static readonly Color Orange = new Color(255, 165, 0, 255);

    /// <summary>
    /// Gets the color with RGBA value of #FF4500FF.
    /// </summary>
    public static readonly Color OrangeRed = new Color(255, 69, 0, 255);

    /// <summary>
    /// Gets the color with RGBA value of #DA70D6FF.
    /// </summary>
    public static readonly Color Orchid = new Color(218, 112, 214, 255);

    /// <summary>
    /// Gets the color with RGBA value of #EEE8AAFF.
    /// </summary>
    public static readonly Color PaleGoldenrod = new Color(238, 232, 170, 255);

    /// <summary>
    /// Gets the color with RGBA value of #98FB98FF.
    /// </summary>
    public static readonly Color PaleGreen = new Color(152, 251, 152, 255);

    /// <summary>
    /// Gets the color with RGBA value of #AFEEEEFF.
    /// </summary>
    public static readonly Color PaleTurquoise = new Color(175, 238, 238, 255);

    /// <summary>
    /// Gets the color with RGBA value of #DB7093FF.
    /// </summary>
    public static readonly Color PaleVioletRed = new Color(219, 112, 147, 255);

    /// <summary>
    /// Gets the color with RGBA value of #FFEFD5FF.
    /// </summary>
    public static readonly Color PapayaWhip = new Color(255, 239, 213, 255);

    /// <summary>
    /// Gets the color with RGBA value of #FFDAB9FF.
    /// </summary>
    public static readonly Color PeachPuff = new Color(255, 218, 185, 255);

    /// <summary>
    /// Gets the color with RGBA value of #CD853FFF.
    /// </summary>
    public static readonly Color Peru = new Color(205, 133, 63, 255);

    /// <summary>
    /// Gets the color with RGBA value of #FFC0CBFF.
    /// </summary>
    public static readonly Color Pink = new Color(255, 192, 203, 255);

    /// <summary>
    /// Gets the color with RGBA value of #DDA0DDFF.
    /// </summary>
    public static readonly Color Plum = new Color(221, 160, 221, 255);

    /// <summary>
    /// Gets the color with RGBA value of #B0E0E6FF.
    /// </summary>
    public static readonly Color PowderBlue = new Color(176, 224, 230, 255);

    /// <summary>
    /// Gets the color with RGBA value of #800080FF.
    /// </summary>
    public static readonly Color Purple = new Color(128, 0, 128, 255);

    /// <summary>
    /// Gets the color with RGBA value of #FF0000FF.
    /// </summary>
    public static readonly Color Red = new Color(255, 0, 0, 255);

    /// <summary>
    /// Gets the color with RGBA value of #BC8F8FFF.
    /// </summary>
    public static readonly Color RosyBrown = new Color(188, 143, 143, 255);

    /// <summary>
    /// Gets the color with RGBA value of #4169E1FF.
    /// </summary>
    public static readonly Color RoyalBlue = new Color(65, 105, 225, 255);

    /// <summary>
    /// Gets the color with RGBA value of #8B4513FF.
    /// </summary>
    public static readonly Color SaddleBrown = new Color(139, 69, 19, 255);

    /// <summary>
    /// Gets the color with RGBA value of #FA8072FF.
    /// </summary>
    public static readonly Color Salmon = new Color(250, 128, 114, 255);

    /// <summary>
    /// Gets the color with RGBA value of #F4A460FF.
    /// </summary>
    public static readonly Color SandyBrown = new Color(244, 164, 96, 255);

    /// <summary>
    /// Gets the color with RGBA value of #2E8B57FF.
    /// </summary>
    public static readonly Color SeaGreen = new Color(46, 139, 87, 255);

    /// <summary>
    /// Gets the color with RGBA value of #FFF5EEFF.
    /// </summary>
    public static readonly Color SeaShell = new Color(255, 245, 238, 255);

    /// <summary>
    /// Gets the color with RGBA value of #A0522DFF.
    /// </summary>
    public static readonly Color Sienna = new Color(160, 82, 45, 255);

    /// <summary>
    /// Gets the color with RGBA value of #C0C0C0FF.
    /// </summary>
    public static readonly Color Silver = new Color(192, 192, 192, 255);

    /// <summary>
    /// Gets the color with RGBA value of #87CEEBFF.
    /// </summary>
    public static readonly Color SkyBlue = new Color(135, 206, 235, 255);

    /// <summary>
    /// Gets the color with RGBA value of #6A5ACDFF.
    /// </summary>
    public static readonly Color SlateBlue = new Color(106, 90, 205, 255);

    /// <summary>
    /// Gets the color with RGBA value of #708090FF.
    /// </summary>
    public static readonly Color SlateGray = new Color(112, 128, 144, 255);

    /// <summary>
    /// Gets the color with RGBA value of #FFFAFAFF.
    /// </summary>
    public static readonly Color Snow = new Color(255, 250, 250, 255);

    /// <summary>
    /// Gets the color with RGBA value of #00FF7FFF.
    /// </summary>
    public static readonly Color SpringGreen = new Color(0, 255, 127, 255);

    /// <summary>
    /// Gets the color with RGBA value of #4682B4FF.
    /// </summary>
    public static readonly Color SteelBlue = new Color(70, 130, 180, 255);

    /// <summary>
    /// Gets the color with RGBA value of #D2B48CFF.
    /// </summary>
    public static readonly Color Tan = new Color(210, 180, 140, 255);

    /// <summary>
    /// Gets the color with RGBA value of #008080FF.
    /// </summary>
    public static readonly Color Teal = new Color(0, 128, 128, 255);

    /// <summary>
    /// Gets the color with RGBA value of #D8BFD8FF.
    /// </summary>
    public static readonly Color Thistle = new Color(216, 191, 216, 255);

    /// <summary>
    /// Gets the color with RGBA value of #FF6347FF.
    /// </summary>
    public static readonly Color Tomato = new Color(255, 99, 71, 255);

    /// <summary>
    /// Gets the color with RGBA value of #FFFFFF00.
    /// </summary>
    public static readonly Color Transparent = new Color(255, 255, 255, 0);

    /// <summary>
    /// Gets the color with RGBA value of #40E0D0FF.
    /// </summary>
    public static readonly Color Turquoise = new Color(64, 224, 208, 255);

    /// <summary>
    /// Gets the color with RGBA value of #EE82EEFF.
    /// </summary>
    public static readonly Color Violet = new Color(238, 130, 238, 255);

    /// <summary>
    /// Gets the color with RGBA value of #F5DEB3FF.
    /// </summary>
    public static readonly Color Wheat = new Color(245, 222, 179, 255);

    /// <summary>
    /// Gets the color with RGBA value of #FFFFFFFF.
    /// </summary>
    public static readonly Color White = new Color(255, 255, 255, 255);

    /// <summary>
    /// Gets the color with RGBA value of #F5F5F5FF.
    /// </summary>
    public static readonly Color WhiteSmoke = new Color(245, 245, 245, 255);

    /// <summary>
    /// Gets the color with RGBA value of #FFFF00FF.
    /// </summary>
    public static readonly Color Yellow = new Color(255, 255, 0, 255);

    /// <summary>
    /// Gets the color with RGBA value of #9ACD32FF.
    /// </summary>
    public static readonly Color YellowGreen = new Color(154, 205, 50, 255);

    #endregion Named Colors

    #region Lifecycle

    public Color(uint value)
    {
        Unsafe.SkipInit(out this);

        _value = value;
    }

    public Color(byte value) : this(value, value, value, value)
    {
    }

    public Color(byte red, byte green, byte blue, byte alpha = 255)
    {
        Unsafe.SkipInit(out this);

        R = red;
        G = green;
        B = blue;
        A = alpha;
    }

    public Color(int red, int green, int blue, int alpha = 255)
    {
        Unsafe.SkipInit(out this);

        R = red.ClampToByte();
        G = green.ClampToByte();
        B = blue.ClampToByte();
        A = alpha.ClampToByte();
    }

    public Color(ReadOnlySpan<byte> values)
    {
        if (values.Length < Count)
        {
            throw new ArgumentOutOfRangeException(nameof(values));
        }

        this = Unsafe.ReadUnaligned<Color>(ref MemoryMarshal.GetReference(values));
    }

    internal static byte GetElement(Color color, int index)
    {
        if ((uint)index >= Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        return GetElementUnsafe(ref color, index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static byte GetElementUnsafe(ref Color vector, int index)
    {
        Debug.Assert(index is >= 0 and < Count);
        return Unsafe.Add(ref Unsafe.As<Color, byte>(ref vector), index);
    }

    internal static Color WithElement(Color vector, int index, byte value)
    {
        if ((uint)index >= Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        Color result = vector;
        SetElementUnsafe(ref result, index, value);
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void SetElementUnsafe(ref Color vector, int index, byte value)
    {
        Debug.Assert(index is >= 0 and < Count);
        Unsafe.Add(ref Unsafe.As<Color, byte>(ref vector), index) = value;
    }

    #endregion Lifecycle

    #region Equality

    public bool Equals(Color other) => _value == other._value;

    public override bool Equals(object? obj) => obj is Color other && Equals(other);

    public override int GetHashCode() => _value.GetHashCode();

    #endregion Equality

    #region Queries

    public readonly void Deconstruct(out byte red, out byte green, out byte blue, out byte alpha)
    {
        red = R;
        green = G;
        blue = B;
        alpha = A;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Color WithRed(byte red) => new(red, G, B, A);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Color WithGreen(byte green) => new(R, green, B, A);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Color WithBlue(byte blue) => new(R, G, blue, A);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Color WithAlpha(byte alpha) => new(R, G, B, alpha);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Color Clamp(Color min, Color max)
    {
        return new Color(
            byte.Clamp(R, min.R, max.R),
            byte.Clamp(G, min.G, max.G),
            byte.Clamp(B, min.B, max.B),
            byte.Clamp(A, min.A, max.A));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Color Negate()
    {
        return new Color(255 - R, 255 - G, 255 - B, A);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color Clamp(Color value, Color min, Color max) => value.Clamp(min, max);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color Min(Color lhs, Color rhs)
    {
        return new Color(
            lhs.R < rhs.R ? lhs.R : rhs.R,
            lhs.G < rhs.G ? lhs.G : rhs.G,
            lhs.B < rhs.B ? lhs.B : rhs.B,
            lhs.A < rhs.A ? lhs.A : rhs.A);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color Max(Color lhs, Color rhs)
    {
        return new Color(
            lhs.R > rhs.R ? lhs.R : rhs.R,
            lhs.G > rhs.G ? lhs.G : rhs.G,
            lhs.B > rhs.B ? lhs.B : rhs.B,
            lhs.A > rhs.A ? lhs.A : rhs.A);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color Negate(Color value) => value.Negate();

    public override string ToString() => $"({R}, {G}, {B}, {A})";

    #endregion Queries

    #region Operators

    public static bool operator ==(Color lhs, Color rhs) => lhs._value == rhs._value;

    public static bool operator !=(Color lhs, Color rhs) => lhs._value != rhs._value;

    public static Color operator +(Color lhs, Color rhs)
    {
        return new Color(lhs.R + rhs.R, lhs.G + rhs.G, lhs.B + rhs.B, lhs.A + rhs.A);
    }

    public static implicit operator Color(uint value) => new(value);

    #endregion Operators

    #region Copy

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void CopyTo(byte[]? destination) => CopyTo(destination, 0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void CopyTo(byte[]? destination, int index)
    {
        if (destination is null)
        {
            throw new NullReferenceException(nameof(destination));
        }

        if (index < 0 || index >= destination.Length)
        {
            throw new IndexOutOfRangeException();
        }

        if (destination.Length - index < Count)
        {
            throw new ArgumentOutOfRangeException(nameof(destination));
        }

        destination[index] = R;
        destination[index + 1] = G;
        destination[index + 2] = B;
        destination[index + 3] = A;
    }

    public readonly void CopyTo(Span<byte> destination)
    {
        if (destination.Length < Count)
        {
            throw new ArgumentOutOfRangeException(nameof(destination));
        }

        Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(destination), this);
    }

    public readonly bool TryCopyTo(Span<byte> destination)
    {
        if (destination.Length < Count)
        {
            return false;
        }

        Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(destination), this);

        return true;
    }

    #endregion Copy
}
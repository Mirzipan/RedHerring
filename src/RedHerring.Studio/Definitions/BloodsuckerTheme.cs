using System.Numerics;
using ImGuiNET;
using RedHerring.Clues;

namespace RedHerring.Studio.Definitions;

[DefinitionType(typeof(ThemeDefinition))]
public sealed class BloodsuckerTheme : ThemeDefinition
{
    public override float Alpha => 1.0f;
    public override float DisabledAlpha => 0.6000000238418579f;
    public override Vector2 WindowPadding => new(8.0f, 8.0f);
    public override float WindowRounding => 0.0f;
    public override float WindowBorderSize => 1.0f;
    public override Vector2 WindowMinSize => new(32.0f, 32.0f);
    public override Vector2 WindowTitleAlign => new(0.0f, 0.5f);
    public override ImGuiDir WindowMenuButtonPosition => ImGuiDir.Left;
    public override float ChildRounding => 0.0f;
    public override float ChildBorderSize => 1.0f;
    public override float PopupRounding => 0.0f;
    public override float PopupBorderSize => 1.0f;
    public override Vector2 FramePadding => new(4.0f, 3.0f);
    public override float FrameRounding => 4.0f;
    public override float FrameBorderSize => 0.0f;
    public override Vector2 ItemSpacing => new(8.0f, 4.0f);
    public override Vector2 ItemInnerSpacing => new(4.0f, 4.0f);
    public override Vector2 CellPadding => new(4.0f, 2.0f);
    public override float IndentSpacing => 21.0f;
    public override float ColumnsMinSpacing => 6.0f;
    public override float ScrollbarSize => 14.0f;
    public override float ScrollbarRounding => 9.0f;
    public override float GrabMinSize => 10.0f;
    public override float GrabRounding => 4.0f;
    public override float TabRounding => 4.0f;
    public override float TabBorderSize => 0.0f;
    public override float TabMinWidthForCloseButton => 0.0f;
    public override ImGuiDir ColorButtonPosition => ImGuiDir.Right;
    public override Vector2 ButtonTextAlign => new(0.5f, 0.5f);
    public override Vector2 SelectableTextAlign => new(0.0f, 0.0f);

    public BloodsuckerTheme() : base("Bloodsucker", false)
    {
        Colors[(int)ImGuiCol.Text] =
            new Vector4(0.9764705896377563f, 0.9490196108818054f, 0.95686274766922f, 1.0f);
        Colors[(int)ImGuiCol.TextDisabled] =
            new Vector4(0.4666666686534882f, 0.3568627536296844f, 0.4196078479290009f, 1.0f);
        Colors[(int)ImGuiCol.WindowBg] =
            new Vector4(0.168627455830574f, 0.1098039224743843f, 0.1490196138620377f, 1.0f);
        Colors[(int)ImGuiCol.ChildBg] =
            new Vector4(0.2196078449487686f, 0.1490196138620377f, 0.1764705926179886f, 1.0f);
        Colors[(int)ImGuiCol.PopupBg] = new Vector4(0.0784313753247261f, 0.0784313753247261f, 0.0784313753247261f,
            0.9399999976158142f);
        Colors[(int)ImGuiCol.Border] =
            new Vector4(0.1176470592617989f, 0.0784313753247261f, 0.09803921729326248f, 1.0f);
        Colors[(int)ImGuiCol.BorderShadow] = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
        Colors[(int)ImGuiCol.FrameBg] =
            new Vector4(0.2862745225429535f, 0.2000000029802322f, 0.2470588237047195f, 1.0f);
        Colors[(int)ImGuiCol.FrameBgHovered] =
            new Vector4(0.2784313857555389f, 0.1176470592617989f, 0.2000000029802322f, 1.0f);
        Colors[(int)ImGuiCol.FrameBgActive] =
            new Vector4(0.1372549086809158f, 0.08627451211214066f, 0.1176470592617989f, 1.0f);
        Colors[(int)ImGuiCol.TitleBg] = new Vector4(0.1372549086809158f, 0.08627451211214066f,
            0.1176470592617989f, 0.6509804129600525f);
        Colors[(int)ImGuiCol.TitleBgActive] =
            new Vector4(0.1176470592617989f, 0.0784313753247261f, 0.09803921729326248f, 1.0f);
        Colors[(int)ImGuiCol.TitleBgCollapsed] = new Vector4(0.0f, 0.0f, 0.0f, 0.5099999904632568f);
        Colors[(int)ImGuiCol.MenuBarBg] =
            new Vector4(0.2196078449487686f, 0.1490196138620377f, 0.1764705926179886f, 1.0f);
        Colors[(int)ImGuiCol.ScrollbarBg] = new Vector4(0.01960784383118153f, 0.01960784383118153f,
            0.01960784383118153f, 0.3899999856948853f);
        Colors[(int)ImGuiCol.ScrollbarGrab] =
            new Vector4(0.2862745225429535f, 0.2000000029802322f, 0.2470588237047195f, 1.0f);
        Colors[(int)ImGuiCol.ScrollbarGrabHovered] =
            new Vector4(0.2470588237047195f, 0.1764705926179886f, 0.2196078449487686f, 1.0f);
        Colors[(int)ImGuiCol.ScrollbarGrabActive] =
            new Vector4(0.3098039329051971f, 0.08627451211214066f, 0.2078431397676468f, 1.0f);
        Colors[(int)ImGuiCol.CheckMark] = new Vector4(1.0f, 0.2784313857555389f, 0.5568627715110779f, 1.0f);
        Colors[(int)ImGuiCol.SliderGrab] = new Vector4(1.0f, 0.2784313857555389f, 0.5568627715110779f, 1.0f);
        Colors[(int)ImGuiCol.SliderGrabActive] =
            new Vector4(1.0f, 0.3686274588108063f, 0.6078431606292725f, 1.0f);
        Colors[(int)ImGuiCol.Button] =
            new Vector4(0.2862745225429535f, 0.2000000029802322f, 0.2470588237047195f, 1.0f);
        Colors[(int)ImGuiCol.ButtonHovered] = new Vector4(1.0f, 0.2784313857555389f, 0.5568627715110779f, 1.0f);
        Colors[(int)ImGuiCol.ButtonActive] =
            new Vector4(0.9764705896377563f, 0.05882352963089943f, 0.529411792755127f, 1.0f);
        Colors[(int)ImGuiCol.Header] = new Vector4(0.2862745225429535f, 0.2000000029802322f, 0.2470588237047195f,
            0.5490196347236633f);
        Colors[(int)ImGuiCol.HeaderHovered] = new Vector4(0.9764705896377563f, 0.2588235437870026f,
            0.5882353186607361f, 0.800000011920929f);
        Colors[(int)ImGuiCol.HeaderActive] =
            new Vector4(0.9764705896377563f, 0.2588235437870026f, 0.5882353186607361f, 1.0f);
        Colors[(int)ImGuiCol.Separator] =
            new Vector4(0.2862745225429535f, 0.2000000029802322f, 0.2470588237047195f, 1.0f);
        Colors[(int)ImGuiCol.SeparatorHovered] = new Vector4(0.7490196228027344f, 0.09803921729326248f,
            0.3960784375667572f, 0.7803921699523926f);
        Colors[(int)ImGuiCol.SeparatorActive] =
            new Vector4(0.7490196228027344f, 0.09803921729326248f, 0.4000000059604645f, 1.0f);
        Colors[(int)ImGuiCol.ResizeGrip] = new Vector4(0.9764705896377563f, 0.2588235437870026f,
            0.5882353186607361f, 0.250980406999588f);
        Colors[(int)ImGuiCol.ResizeGripHovered] = new Vector4(0.9764705896377563f, 0.2588235437870026f,
            0.5882353186607361f, 0.6705882549285889f);
        Colors[(int)ImGuiCol.ResizeGripActive] = new Vector4(0.9764705896377563f, 0.2588235437870026f,
            0.5882353186607361f, 0.9490196108818054f);
        Colors[(int)ImGuiCol.Tab] =
            new Vector4(0.168627455830574f, 0.1098039224743843f, 0.1490196138620377f, 1.0f);
        Colors[(int)ImGuiCol.TabHovered] = new Vector4(0.9764705896377563f, 0.2588235437870026f,
            0.5882353186607361f, 0.800000011920929f);
        Colors[(int)ImGuiCol.TabActive] =
            new Vector4(0.2862745225429535f, 0.2000000029802322f, 0.2470588237047195f, 1.0f);
        Colors[(int)ImGuiCol.TabUnfocused] =
            new Vector4(0.168627455830574f, 0.1098039224743843f, 0.1490196138620377f, 1.0f);
        Colors[(int)ImGuiCol.TabUnfocusedActive] =
            new Vector4(0.168627455830574f, 0.1098039224743843f, 0.1490196138620377f, 1.0f);
        Colors[(int)ImGuiCol.PlotLines] =
            new Vector4(0.6078431606292725f, 0.6078431606292725f, 0.6078431606292725f, 1.0f);
        Colors[(int)ImGuiCol.PlotLinesHovered] =
            new Vector4(0.3490196168422699f, 1.0f, 0.4274509847164154f, 1.0f);
        Colors[(int)ImGuiCol.PlotHistogram] = new Vector4(0.0f, 0.8980392217636108f, 0.6980392336845398f, 1.0f);
        Colors[(int)ImGuiCol.PlotHistogramHovered] = new Vector4(0.0f, 1.0f, 0.6000000238418579f, 1.0f);
        Colors[(int)ImGuiCol.TableHeaderBg] =
            new Vector4(0.2000000029802322f, 0.1882352977991104f, 0.1882352977991104f, 1.0f);
        Colors[(int)ImGuiCol.TableBorderStrong] =
            new Vector4(0.3490196168422699f, 0.3098039329051971f, 0.3098039329051971f, 1.0f);
        Colors[(int)ImGuiCol.TableBorderLight] =
            new Vector4(0.2470588237047195f, 0.2274509817361832f, 0.2274509817361832f, 1.0f);
        Colors[(int)ImGuiCol.TableRowBg] = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
        Colors[(int)ImGuiCol.TableRowBgAlt] = new Vector4(1.0f, 1.0f, 1.0f, 0.05999999865889549f);
        Colors[(int)ImGuiCol.TextSelectedBg] = new Vector4(0.9764705896377563f, 0.2588235437870026f,
            0.5882353186607361f, 0.3490196168422699f);
        Colors[(int)ImGuiCol.DragDropTarget] = new Vector4(0.0f, 1.0f, 1.0f, 0.9019607901573181f);
        Colors[(int)ImGuiCol.NavHighlight] =
            new Vector4(0.9764705896377563f, 0.2588235437870026f, 0.5882353186607361f, 1.0f);
        Colors[(int)ImGuiCol.NavWindowingHighlight] = new Vector4(1.0f, 1.0f, 1.0f, 0.699999988079071f);
        Colors[(int)ImGuiCol.NavWindowingDimBg] = new Vector4(0.800000011920929f, 0.800000011920929f,
            0.800000011920929f, 0.2000000029802322f);
        Colors[(int)ImGuiCol.ModalWindowDimBg] = new Vector4(0.800000011920929f, 0.800000011920929f,
            0.800000011920929f, 0.3499999940395355f);
    }
}
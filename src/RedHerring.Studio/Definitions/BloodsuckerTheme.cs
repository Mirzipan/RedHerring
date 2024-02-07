using System.Numerics;
using ImGuiNET;
using RedHerring.Clues;

namespace RedHerring.Studio.Definitions;

[DefinitionType(typeof(ThemeDefinition))]
public sealed class BloodsuckerTheme() : ThemeDefinition("Bloodsucker", false)
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
}
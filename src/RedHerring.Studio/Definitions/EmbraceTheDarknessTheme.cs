using System.Numerics;
using ImGuiNET;
using RedHerring.Clues;

namespace RedHerring.Studio.Definitions;

[DefinitionType(typeof(ThemeDefinition))]
public sealed class EmbraceTheDarknessTheme() : ThemeDefinition("Embrace the Darkness", true)
{
    public override float Alpha => 1.0f;
    public override float DisabledAlpha => 1.0f;
    public override Vector2 WindowPadding => new(12.0f, 12.0f);
    public override float WindowRounding => 2.0f;
    public override float WindowBorderSize => 0.0f;
    public override Vector2 WindowMinSize => new(20.0f, 20.0f);
    public override Vector2 WindowTitleAlign => new(0.5f, 0.5f);
    public override ImGuiDir WindowMenuButtonPosition => ImGuiDir.None;
    public override float ChildRounding => 4.0f;
    public override float ChildBorderSize => 1.0f;
    public override float PopupRounding => 4.0f;
    public override float PopupBorderSize => 1.0f;
    public override Vector2 FramePadding => new(6.0f, 6.0f);
    public override float FrameRounding => 4.0f;
    public override float FrameBorderSize => 0.0f;
    public override Vector2 ItemSpacing => new(12.0f, 6.0f);
    public override Vector2 ItemInnerSpacing => new(6.0f, 3.0f);
    public override Vector2 CellPadding => new(12.0f, 6.0f);
    public override float IndentSpacing => 20.0f;
    public override float ColumnsMinSpacing => 6.0f;
    public override float ScrollbarSize => 12.0f;
    public override float ScrollbarRounding => 4.0f;
    public override float GrabMinSize => 12.0f;
    public override float GrabRounding => 4.0f;
    public override float TabRounding => 4.0f;
    public override float TabBorderSize => 0.0f;
    public override float TabMinWidthForCloseButton => 0.0f;
    public override ImGuiDir ColorButtonPosition => ImGuiDir.Right;
    public override Vector2 ButtonTextAlign => new(0.5f, 0.5f);
    public override Vector2 SelectableTextAlign => new(0.0f, 0.0f);
}
using System.Numerics;
using ImGuiNET;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.ImGui;

public static class UITheme
{
    public static void EmbraceTheDarkness()
    {
        ImGuiStylePtr style = Gui.GetStyle();
        style.Colors[(int)ImGuiCol.WindowBg] = new Vector4(0.10f, 0.10f, 0.10f, 1.00f);
        style.Colors[(int)ImGuiCol.MenuBarBg] = new Vector4(0.14f, 0.14f, 0.14f, 1.00f);
        style.Colors[(int)ImGuiCol.ChildBg] = new Vector4(0.00f, 0.00f, 0.00f, 0.00f);
        style.Colors[(int)ImGuiCol.DragDropTarget] = new Vector4(0.33f, 0.67f, 0.86f, 1.00f);
        style.Colors[(int)ImGuiCol.ModalWindowDimBg] = new Vector4(1.00f, 0.00f, 0.00f, 0.35f);

        // Border
        style.Colors[(int)ImGuiCol.Border] = new Vector4(0.19f, 0.19f, 0.19f, 0.29f);
        style.Colors[(int)ImGuiCol.BorderShadow] = new Vector4(0.00f, 0.00f, 0.00f, 0.24f);

        // Text
        style.Colors[(int)ImGuiCol.Text] = new Vector4(1.00f, 1.00f, 1.00f, 1.00f);
        style.Colors[(int)ImGuiCol.TextDisabled] = new Vector4(0.50f, 0.50f, 0.50f, 1.00f);
        style.Colors[(int)ImGuiCol.TextSelectedBg] = new Vector4(0.20f, 0.22f, 0.23f, 1.00f);

        // Header
        style.Colors[(int)ImGuiCol.Header] = new Vector4(0.00f, 0.00f, 0.00f, 0.52f);
        style.Colors[(int)ImGuiCol.HeaderHovered] = new Vector4(0.00f, 0.00f, 0.00f, 0.36f);
        style.Colors[(int)ImGuiCol.HeaderActive] = new Vector4(0.20f, 0.22f, 0.23f, 0.33f);

        // Button
        style.Colors[(int)ImGuiCol.Button] = new Vector4(0.05f, 0.05f, 0.05f, 0.54f);
        style.Colors[(int)ImGuiCol.ButtonHovered] = new Vector4(0.19f, 0.19f, 0.19f, 0.54f);
        style.Colors[(int)ImGuiCol.ButtonActive] = new Vector4(0.20f, 0.22f, 0.23f, 1.00f);
        style.Colors[(int)ImGuiCol.CheckMark] = new Vector4(0.33f, 0.67f, 0.86f, 1.00f);

        // Popup
        style.Colors[(int)ImGuiCol.PopupBg] = new Vector4(0.19f, 0.19f, 0.19f, 0.92f);

        // Slider
        style.Colors[(int)ImGuiCol.SliderGrab] = new Vector4(0.34f, 0.34f, 0.34f, 0.54f);
        style.Colors[(int)ImGuiCol.SliderGrabActive] = new Vector4(0.56f, 0.56f, 0.56f, 0.54f);

        // Frame
        style.Colors[(int)ImGuiCol.FrameBg] = new Vector4(0.05f, 0.05f, 0.05f, 0.54f);
        style.Colors[(int)ImGuiCol.FrameBgHovered] = new Vector4(0.19f, 0.19f, 0.19f, 0.54f);
        style.Colors[(int)ImGuiCol.FrameBgActive] = new Vector4(0.20f, 0.22f, 0.23f, 1.00f);

        // Tab
        style.Colors[(int)ImGuiCol.Tab] = new Vector4(0.00f, 0.00f, 0.00f, 0.52f);
        style.Colors[(int)ImGuiCol.TabHovered] = new Vector4(0.14f, 0.14f, 0.14f, 1.00f);
        style.Colors[(int)ImGuiCol.TabActive] = new Vector4(0.20f, 0.20f, 0.20f, 0.36f);
        style.Colors[(int)ImGuiCol.TabUnfocused] = new Vector4(0.00f, 0.00f, 0.00f, 0.52f);
        style.Colors[(int)ImGuiCol.TabUnfocusedActive] = new Vector4(0.14f, 0.14f, 0.14f, 1.00f);

        // Title
        style.Colors[(int)ImGuiCol.TitleBg] = new Vector4(0.00f, 0.00f, 0.00f, 1.00f);
        style.Colors[(int)ImGuiCol.TitleBgActive] = new Vector4(0.06f, 0.06f, 0.06f, 1.00f);
        style.Colors[(int)ImGuiCol.TitleBgCollapsed] = new Vector4(0.00f, 0.00f, 0.00f, 1.00f);

        // Scrollbar
        style.Colors[(int)ImGuiCol.ScrollbarBg] = new Vector4(0.05f, 0.05f, 0.05f, 0.54f);
        style.Colors[(int)ImGuiCol.ScrollbarGrab] = new Vector4(0.34f, 0.34f, 0.34f, 0.54f);
        style.Colors[(int)ImGuiCol.ScrollbarGrabHovered] = new Vector4(0.40f, 0.40f, 0.40f, 0.54f);
        style.Colors[(int)ImGuiCol.ScrollbarGrabActive] = new Vector4(0.56f, 0.56f, 0.56f, 0.54f);

        // Separator
        style.Colors[(int)ImGuiCol.Separator] = new Vector4(0.28f, 0.28f, 0.28f, 0.29f);
        style.Colors[(int)ImGuiCol.SeparatorHovered] = new Vector4(0.44f, 0.44f, 0.44f, 0.29f);
        style.Colors[(int)ImGuiCol.SeparatorActive] = new Vector4(0.40f, 0.44f, 0.47f, 1.00f);

        // Resize Grip
        style.Colors[(int)ImGuiCol.ResizeGrip] = new Vector4(0.28f, 0.28f, 0.28f, 0.29f);
        style.Colors[(int)ImGuiCol.ResizeGripHovered] = new Vector4(0.44f, 0.44f, 0.44f, 0.29f);
        style.Colors[(int)ImGuiCol.ResizeGripActive] = new Vector4(0.40f, 0.44f, 0.47f, 1.00f);

        // Docking
        style.Colors[(int)ImGuiCol.DockingPreview] = new Vector4(0.33f, 0.67f, 0.86f, 1.00f);
        style.Colors[(int)ImGuiCol.DockingEmptyBg] = new Vector4(1.00f, 0.00f, 0.00f, 1.00f);

        // Table
        style.Colors[(int)ImGuiCol.TableHeaderBg] = new Vector4(0.00f, 0.00f, 0.00f, 0.52f);
        style.Colors[(int)ImGuiCol.TableBorderStrong] = new Vector4(0.00f, 0.00f, 0.00f, 0.52f);
        style.Colors[(int)ImGuiCol.TableBorderLight] = new Vector4(0.28f, 0.28f, 0.28f, 0.29f);
        style.Colors[(int)ImGuiCol.TableRowBg] = new Vector4(0.00f, 0.00f, 0.00f, 0.00f);
        style.Colors[(int)ImGuiCol.TableRowBgAlt] = new Vector4(1.00f, 1.00f, 1.00f, 0.06f);

        // Plot
        style.Colors[(int)ImGuiCol.PlotLines] = new Vector4(1.00f, 0.00f, 0.00f, 1.00f);
        style.Colors[(int)ImGuiCol.PlotLinesHovered] = new Vector4(1.00f, 0.00f, 0.00f, 1.00f);
        style.Colors[(int)ImGuiCol.PlotHistogram] = new Vector4(1.00f, 0.00f, 0.00f, 1.00f);
        style.Colors[(int)ImGuiCol.PlotHistogramHovered] = new Vector4(1.00f, 0.00f, 0.00f, 1.00f);

        // Nav
        style.Colors[(int)ImGuiCol.NavHighlight] = new Vector4(1.00f, 0.00f, 0.00f, 1.00f);
        style.Colors[(int)ImGuiCol.NavWindowingHighlight] = new Vector4(1.00f, 0.00f, 0.00f, 0.70f);
        style.Colors[(int)ImGuiCol.NavWindowingDimBg] = new Vector4(1.00f, 0.00f, 0.00f, 0.20f);

        style.WindowPadding = new Vector2(8.00f, 8.00f);
        style.WindowMinSize = new Vector2(32.00f, 32.00f);
        style.WindowTitleAlign = new Vector2(0.0f, 0.5f);
        style.WindowMenuButtonPosition = ImGuiDir.Left;

        style.FramePadding = new Vector2(5.00f, 2.00f);
        style.CellPadding = new Vector2(6.00f, 6.00f);
        style.ItemSpacing = new Vector2(6.00f, 6.00f);
        style.ItemInnerSpacing = new Vector2(6.00f, 6.00f);
        style.TouchExtraPadding = new Vector2(0.00f, 0.00f);

        style.ColorButtonPosition = ImGuiDir.Right;
        style.ButtonTextAlign = new Vector2(0.5f, 0.5f);
        style.SelectableTextAlign = new Vector2(0.0f, 0.0f);

        style.IndentSpacing = 25;
        style.ScrollbarSize = 15;
        style.GrabMinSize = 10;
        style.WindowBorderSize = 1;
        style.ChildBorderSize = 1;
        style.PopupBorderSize = 1;
        style.FrameBorderSize = 1;
        style.TabBorderSize = 1;
        style.WindowRounding = 7;
        style.ChildRounding = 4;
        style.FrameRounding = 3;
        style.PopupRounding = 4;
        style.ScrollbarRounding = 9;
        style.GrabRounding = 3;
        style.LogSliderDeadzone = 4;
        style.TabRounding = 4;
        style.TabMinWidthForCloseButton = 0.4f;

        style.Alpha = 1f;
        style.DisabledAlpha = 0.6f;
    }

    public static void CrimsonRivers()
    {
        ImGuiStylePtr style = Gui.GetStyle();
        style.Colors[(int)ImGuiCol.WindowBg] = new Vector4(0.00f, 0.00f, 0.00f, 0.94f);
        style.Colors[(int)ImGuiCol.MenuBarBg] = new Vector4(0.14f, 0.14f, 0.14f, 1.00f);
        style.Colors[(int)ImGuiCol.ChildBg] = new Vector4(0.00f, 0.00f, 0.00f, 0.00f);
        style.Colors[(int)ImGuiCol.DragDropTarget] = new Vector4(1.00f, 1.00f, 0.00f, 0.90f);
        style.Colors[(int)ImGuiCol.ModalWindowDimBg] = new Vector4(0.80f, 0.80f, 0.80f, 0.35f);

        // Border
        style.Colors[(int)ImGuiCol.Border] = new Vector4(0.00f, 0.00f, 0.00f, 0.50f);
        style.Colors[(int)ImGuiCol.BorderShadow] = new Vector4(0.00f, 0.00f, 0.00f, 0.00f);

        // Text
        style.Colors[(int)ImGuiCol.Text] = new Vector4(0.75f, 0.75f, 0.75f, 1.00f);
        style.Colors[(int)ImGuiCol.TextDisabled] = new Vector4(0.35f, 0.35f, 0.35f, 1.00f);
        style.Colors[(int)ImGuiCol.TextSelectedBg] = new Vector4(0.26f, 0.59f, 0.98f, 0.35f);

        // Header
        style.Colors[(int)ImGuiCol.Header] = new Vector4(0.33f, 0.35f, 0.36f, 0.53f);
        style.Colors[(int)ImGuiCol.HeaderHovered] = new Vector4(0.76f, 0.28f, 0.44f, 0.67f);
        style.Colors[(int)ImGuiCol.HeaderActive] = new Vector4(0.47f, 0.47f, 0.47f, 0.67f);

        // Button
        style.Colors[(int)ImGuiCol.Button] = new Vector4(1.00f, 0.19f, 0.19f, 0.40f);
        style.Colors[(int)ImGuiCol.ButtonHovered] = new Vector4(0.80f, 0.17f, 0.00f, 1.00f);
        style.Colors[(int)ImGuiCol.ButtonActive] = new Vector4(0.89f, 0.00f, 0.19f, 1.00f);
        style.Colors[(int)ImGuiCol.CheckMark] = new Vector4(0.56f, 0.10f, 0.10f, 1.00f);

        // Popup
        style.Colors[(int)ImGuiCol.PopupBg] = new Vector4(0.08f, 0.08f, 0.08f, 0.94f);

        // Slider
        style.Colors[(int)ImGuiCol.SliderGrab] = new Vector4(1.00f, 0.19f, 0.19f, 0.40f);
        style.Colors[(int)ImGuiCol.SliderGrabActive] = new Vector4(0.89f, 0.00f, 0.19f, 1.00f);

        // Frame
        style.Colors[(int)ImGuiCol.FrameBg] = new Vector4(0.00f, 0.00f, 0.00f, 0.54f);
        style.Colors[(int)ImGuiCol.FrameBgHovered] = new Vector4(0.37f, 0.14f, 0.14f, 0.67f);
        style.Colors[(int)ImGuiCol.FrameBgActive] = new Vector4(0.39f, 0.20f, 0.20f, 0.67f);

        // Tab
        style.Colors[(int)ImGuiCol.Tab] = new Vector4(0.07f, 0.07f, 0.07f, 0.51f);
        style.Colors[(int)ImGuiCol.TabHovered] = new Vector4(0.86f, 0.23f, 0.43f, 0.67f);
        style.Colors[(int)ImGuiCol.TabActive] = new Vector4(0.19f, 0.19f, 0.19f, 0.57f);
        style.Colors[(int)ImGuiCol.TabUnfocused] = new Vector4(0.05f, 0.05f, 0.05f, 0.90f);
        style.Colors[(int)ImGuiCol.TabUnfocusedActive] = new Vector4(0.13f, 0.13f, 0.13f, 0.74f);

        // Title
        style.Colors[(int)ImGuiCol.TitleBg] = new Vector4(0.04f, 0.04f, 0.04f, 1.00f);
        style.Colors[(int)ImGuiCol.TitleBgActive] = new Vector4(0.48f, 0.16f, 0.16f, 1.00f);
        style.Colors[(int)ImGuiCol.TitleBgCollapsed] = new Vector4(0.48f, 0.16f, 0.16f, 1.00f);

        // Scrollbar
        style.Colors[(int)ImGuiCol.ScrollbarBg] = new Vector4(0.02f, 0.02f, 0.02f, 0.53f);
        style.Colors[(int)ImGuiCol.ScrollbarGrab] = new Vector4(0.31f, 0.31f, 0.31f, 1.00f);
        style.Colors[(int)ImGuiCol.ScrollbarGrabHovered] = new Vector4(0.41f, 0.41f, 0.41f, 1.00f);
        style.Colors[(int)ImGuiCol.ScrollbarGrabActive] = new Vector4(0.51f, 0.51f, 0.51f, 1.00f);

        // Separator
        style.Colors[(int)ImGuiCol.Separator] = new Vector4(0.32f, 0.32f, 0.32f, 1.00f);
        style.Colors[(int)ImGuiCol.SeparatorHovered] = new Vector4(0.32f, 0.32f, 0.32f, 1.00f);
        style.Colors[(int)ImGuiCol.SeparatorActive] = new Vector4(0.32f, 0.32f, 0.32f, 1.00f);

        // Resize Grip
        style.Colors[(int)ImGuiCol.ResizeGrip] = new Vector4(1.00f, 1.00f, 1.00f, 0.85f);
        style.Colors[(int)ImGuiCol.ResizeGripHovered] = new Vector4(1.00f, 1.00f, 1.00f, 0.60f);
        style.Colors[(int)ImGuiCol.ResizeGripActive] = new Vector4(1.00f, 1.00f, 1.00f, 0.90f);

        // Docking
        style.Colors[(int)ImGuiCol.DockingPreview] = new Vector4(0.47f, 0.47f, 0.47f, 0.47f);
        style.Colors[(int)ImGuiCol.DockingEmptyBg] = new Vector4(0.20f, 0.20f, 0.20f, 1.00f);

        // Table
        style.Colors[(int)ImGuiCol.TableHeaderBg] = new Vector4(0.19f, 0.19f, 0.20f, 1.00f);
        style.Colors[(int)ImGuiCol.TableBorderStrong] = new Vector4(0.31f, 0.31f, 0.35f, 1.00f);
        style.Colors[(int)ImGuiCol.TableBorderLight] = new Vector4(0.23f, 0.23f, 0.25f, 1.00f);
        style.Colors[(int)ImGuiCol.TableRowBg] = new Vector4(0.00f, 0.00f, 0.00f, 0.00f);
        style.Colors[(int)ImGuiCol.TableRowBgAlt] = new Vector4(1.00f, 1.00f, 1.00f, 0.07f);

        // Plot
        style.Colors[(int)ImGuiCol.PlotLines] = new Vector4(0.61f, 0.61f, 0.61f, 1.00f);
        style.Colors[(int)ImGuiCol.PlotLinesHovered] = new Vector4(1.00f, 0.43f, 0.35f, 1.00f);
        style.Colors[(int)ImGuiCol.PlotHistogram] = new Vector4(0.90f, 0.70f, 0.00f, 1.00f);
        style.Colors[(int)ImGuiCol.PlotHistogramHovered] = new Vector4(1.00f, 0.60f, 0.00f, 1.00f);

        // Nav
        style.Colors[(int)ImGuiCol.NavHighlight] = new Vector4(0.26f, 0.59f, 0.98f, 1.00f);
        style.Colors[(int)ImGuiCol.NavWindowingHighlight] = new Vector4(1.00f, 1.00f, 1.00f, 0.70f);
        style.Colors[(int)ImGuiCol.NavWindowingDimBg] = new Vector4(0.80f, 0.80f, 0.80f, 0.20f);

        style.WindowPadding = new Vector2(8.00f, 8.00f);
        style.WindowMinSize = new Vector2(32.00f, 32.00f);
        style.WindowTitleAlign = new Vector2(0.0f, 0.5f);
        style.WindowMenuButtonPosition = ImGuiDir.Left;

        style.FramePadding = new Vector2(5.00f, 2.00f);
        style.CellPadding = new Vector2(6.00f, 6.00f);
        style.ItemSpacing = new Vector2(6.00f, 6.00f);
        style.ItemInnerSpacing = new Vector2(6.00f, 6.00f);
        style.TouchExtraPadding = new Vector2(0.00f, 0.00f);

        style.ColorButtonPosition = ImGuiDir.Right;
        style.ButtonTextAlign = new Vector2(0.5f, 0.5f);
        style.SelectableTextAlign = new Vector2(0.0f, 0.0f);

        style.IndentSpacing = 25;
        style.ScrollbarSize = 15;
        style.GrabMinSize = 10;
        style.WindowBorderSize = 1;
        style.ChildBorderSize = 1;
        style.PopupBorderSize = 1;
        style.FrameBorderSize = 1;
        style.TabBorderSize = 1;
        style.WindowRounding = 7;
        style.ChildRounding = 4;
        style.FrameRounding = 3;
        style.PopupRounding = 4;
        style.ScrollbarRounding = 9;
        style.GrabRounding = 3;
        style.LogSliderDeadzone = 4;
        style.TabRounding = 4;
        style.TabMinWidthForCloseButton = 0.4f;

        style.Alpha = 1f;
        style.DisabledAlpha = 0.6f;
    }

    public static void Bloodsucker()
    {
        var style = Gui.GetStyle();

        style.Alpha = 1.0f;
        style.DisabledAlpha = 0.6000000238418579f;
        style.WindowPadding = new Vector2(8.0f, 8.0f);
        style.WindowRounding = 0.0f;
        style.WindowBorderSize = 1.0f;
        style.WindowMinSize = new Vector2(32.0f, 32.0f);
        style.WindowTitleAlign = new Vector2(0.0f, 0.5f);
        style.WindowMenuButtonPosition = ImGuiDir.Left;
        style.ChildRounding = 0.0f;
        style.ChildBorderSize = 1.0f;
        style.PopupRounding = 0.0f;
        style.PopupBorderSize = 1.0f;
        style.FramePadding = new Vector2(4.0f, 3.0f);
        style.FrameRounding = 4.0f;
        style.FrameBorderSize = 0.0f;
        style.ItemSpacing = new Vector2(8.0f, 4.0f);
        style.ItemInnerSpacing = new Vector2(4.0f, 4.0f);
        style.CellPadding = new Vector2(4.0f, 2.0f);
        style.IndentSpacing = 21.0f;
        style.ColumnsMinSpacing = 6.0f;
        style.ScrollbarSize = 14.0f;
        style.ScrollbarRounding = 9.0f;
        style.GrabMinSize = 10.0f;
        style.GrabRounding = 4.0f;
        style.TabRounding = 4.0f;
        style.TabBorderSize = 0.0f;
        style.TabMinWidthForCloseButton = 0.0f;
        style.ColorButtonPosition = ImGuiDir.Right;
        style.ButtonTextAlign = new Vector2(0.5f, 0.5f);
        style.SelectableTextAlign = new Vector2(0.0f, 0.0f);

        style.Colors[(int)ImGuiCol.Text] =
            new Vector4(0.9764705896377563f, 0.9490196108818054f, 0.95686274766922f, 1.0f);
        style.Colors[(int)ImGuiCol.TextDisabled] =
            new Vector4(0.4666666686534882f, 0.3568627536296844f, 0.4196078479290009f, 1.0f);
        style.Colors[(int)ImGuiCol.WindowBg] =
            new Vector4(0.168627455830574f, 0.1098039224743843f, 0.1490196138620377f, 1.0f);
        style.Colors[(int)ImGuiCol.ChildBg] =
            new Vector4(0.2196078449487686f, 0.1490196138620377f, 0.1764705926179886f, 1.0f);
        style.Colors[(int)ImGuiCol.PopupBg] = new Vector4(0.0784313753247261f, 0.0784313753247261f, 0.0784313753247261f,
            0.9399999976158142f);
        style.Colors[(int)ImGuiCol.Border] =
            new Vector4(0.1176470592617989f, 0.0784313753247261f, 0.09803921729326248f, 1.0f);
        style.Colors[(int)ImGuiCol.BorderShadow] = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
        style.Colors[(int)ImGuiCol.FrameBg] =
            new Vector4(0.2862745225429535f, 0.2000000029802322f, 0.2470588237047195f, 1.0f);
        style.Colors[(int)ImGuiCol.FrameBgHovered] =
            new Vector4(0.2784313857555389f, 0.1176470592617989f, 0.2000000029802322f, 1.0f);
        style.Colors[(int)ImGuiCol.FrameBgActive] =
            new Vector4(0.1372549086809158f, 0.08627451211214066f, 0.1176470592617989f, 1.0f);
        style.Colors[(int)ImGuiCol.TitleBg] = new Vector4(0.1372549086809158f, 0.08627451211214066f,
            0.1176470592617989f, 0.6509804129600525f);
        style.Colors[(int)ImGuiCol.TitleBgActive] =
            new Vector4(0.1176470592617989f, 0.0784313753247261f, 0.09803921729326248f, 1.0f);
        style.Colors[(int)ImGuiCol.TitleBgCollapsed] = new Vector4(0.0f, 0.0f, 0.0f, 0.5099999904632568f);
        style.Colors[(int)ImGuiCol.MenuBarBg] =
            new Vector4(0.2196078449487686f, 0.1490196138620377f, 0.1764705926179886f, 1.0f);
        style.Colors[(int)ImGuiCol.ScrollbarBg] = new Vector4(0.01960784383118153f, 0.01960784383118153f,
            0.01960784383118153f, 0.3899999856948853f);
        style.Colors[(int)ImGuiCol.ScrollbarGrab] =
            new Vector4(0.2862745225429535f, 0.2000000029802322f, 0.2470588237047195f, 1.0f);
        style.Colors[(int)ImGuiCol.ScrollbarGrabHovered] =
            new Vector4(0.2470588237047195f, 0.1764705926179886f, 0.2196078449487686f, 1.0f);
        style.Colors[(int)ImGuiCol.ScrollbarGrabActive] =
            new Vector4(0.3098039329051971f, 0.08627451211214066f, 0.2078431397676468f, 1.0f);
        style.Colors[(int)ImGuiCol.CheckMark] = new Vector4(1.0f, 0.2784313857555389f, 0.5568627715110779f, 1.0f);
        style.Colors[(int)ImGuiCol.SliderGrab] = new Vector4(1.0f, 0.2784313857555389f, 0.5568627715110779f, 1.0f);
        style.Colors[(int)ImGuiCol.SliderGrabActive] =
            new Vector4(1.0f, 0.3686274588108063f, 0.6078431606292725f, 1.0f);
        style.Colors[(int)ImGuiCol.Button] =
            new Vector4(0.2862745225429535f, 0.2000000029802322f, 0.2470588237047195f, 1.0f);
        style.Colors[(int)ImGuiCol.ButtonHovered] = new Vector4(1.0f, 0.2784313857555389f, 0.5568627715110779f, 1.0f);
        style.Colors[(int)ImGuiCol.ButtonActive] =
            new Vector4(0.9764705896377563f, 0.05882352963089943f, 0.529411792755127f, 1.0f);
        style.Colors[(int)ImGuiCol.Header] = new Vector4(0.2862745225429535f, 0.2000000029802322f, 0.2470588237047195f,
            0.5490196347236633f);
        style.Colors[(int)ImGuiCol.HeaderHovered] = new Vector4(0.9764705896377563f, 0.2588235437870026f,
            0.5882353186607361f, 0.800000011920929f);
        style.Colors[(int)ImGuiCol.HeaderActive] =
            new Vector4(0.9764705896377563f, 0.2588235437870026f, 0.5882353186607361f, 1.0f);
        style.Colors[(int)ImGuiCol.Separator] =
            new Vector4(0.2862745225429535f, 0.2000000029802322f, 0.2470588237047195f, 1.0f);
        style.Colors[(int)ImGuiCol.SeparatorHovered] = new Vector4(0.7490196228027344f, 0.09803921729326248f,
            0.3960784375667572f, 0.7803921699523926f);
        style.Colors[(int)ImGuiCol.SeparatorActive] =
            new Vector4(0.7490196228027344f, 0.09803921729326248f, 0.4000000059604645f, 1.0f);
        style.Colors[(int)ImGuiCol.ResizeGrip] = new Vector4(0.9764705896377563f, 0.2588235437870026f,
            0.5882353186607361f, 0.250980406999588f);
        style.Colors[(int)ImGuiCol.ResizeGripHovered] = new Vector4(0.9764705896377563f, 0.2588235437870026f,
            0.5882353186607361f, 0.6705882549285889f);
        style.Colors[(int)ImGuiCol.ResizeGripActive] = new Vector4(0.9764705896377563f, 0.2588235437870026f,
            0.5882353186607361f, 0.9490196108818054f);
        style.Colors[(int)ImGuiCol.Tab] =
            new Vector4(0.168627455830574f, 0.1098039224743843f, 0.1490196138620377f, 1.0f);
        style.Colors[(int)ImGuiCol.TabHovered] = new Vector4(0.9764705896377563f, 0.2588235437870026f,
            0.5882353186607361f, 0.800000011920929f);
        style.Colors[(int)ImGuiCol.TabActive] =
            new Vector4(0.2862745225429535f, 0.2000000029802322f, 0.2470588237047195f, 1.0f);
        style.Colors[(int)ImGuiCol.TabUnfocused] =
            new Vector4(0.168627455830574f, 0.1098039224743843f, 0.1490196138620377f, 1.0f);
        style.Colors[(int)ImGuiCol.TabUnfocusedActive] =
            new Vector4(0.168627455830574f, 0.1098039224743843f, 0.1490196138620377f, 1.0f);
        style.Colors[(int)ImGuiCol.PlotLines] =
            new Vector4(0.6078431606292725f, 0.6078431606292725f, 0.6078431606292725f, 1.0f);
        style.Colors[(int)ImGuiCol.PlotLinesHovered] =
            new Vector4(0.3490196168422699f, 1.0f, 0.4274509847164154f, 1.0f);
        style.Colors[(int)ImGuiCol.PlotHistogram] = new Vector4(0.0f, 0.8980392217636108f, 0.6980392336845398f, 1.0f);
        style.Colors[(int)ImGuiCol.PlotHistogramHovered] = new Vector4(0.0f, 1.0f, 0.6000000238418579f, 1.0f);
        style.Colors[(int)ImGuiCol.TableHeaderBg] =
            new Vector4(0.2000000029802322f, 0.1882352977991104f, 0.1882352977991104f, 1.0f);
        style.Colors[(int)ImGuiCol.TableBorderStrong] =
            new Vector4(0.3490196168422699f, 0.3098039329051971f, 0.3098039329051971f, 1.0f);
        style.Colors[(int)ImGuiCol.TableBorderLight] =
            new Vector4(0.2470588237047195f, 0.2274509817361832f, 0.2274509817361832f, 1.0f);
        style.Colors[(int)ImGuiCol.TableRowBg] = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
        style.Colors[(int)ImGuiCol.TableRowBgAlt] = new Vector4(1.0f, 1.0f, 1.0f, 0.05999999865889549f);
        style.Colors[(int)ImGuiCol.TextSelectedBg] = new Vector4(0.9764705896377563f, 0.2588235437870026f,
            0.5882353186607361f, 0.3490196168422699f);
        style.Colors[(int)ImGuiCol.DragDropTarget] = new Vector4(0.0f, 1.0f, 1.0f, 0.9019607901573181f);
        style.Colors[(int)ImGuiCol.NavHighlight] =
            new Vector4(0.9764705896377563f, 0.2588235437870026f, 0.5882353186607361f, 1.0f);
        style.Colors[(int)ImGuiCol.NavWindowingHighlight] = new Vector4(1.0f, 1.0f, 1.0f, 0.699999988079071f);
        style.Colors[(int)ImGuiCol.NavWindowingDimBg] = new Vector4(0.800000011920929f, 0.800000011920929f,
            0.800000011920929f, 0.2000000029802322f);
        style.Colors[(int)ImGuiCol.ModalWindowDimBg] = new Vector4(0.800000011920929f, 0.800000011920929f,
            0.800000011920929f, 0.3499999940395355f);
    }
}
using System.Numerics;
using ImGuiNET;
using RedHerring.Clues;

namespace RedHerring.Studio.Definitions;

public partial class ThemeDefinition : Definition
{
    public virtual float Alpha => 1.0f;
    public virtual float DisabledAlpha => 1.0f;
    public virtual Vector2 WindowPadding => new(12.0f, 12.0f);
    public virtual float WindowRounding => 2.0f;
    public virtual float WindowBorderSize => 0.0f;
    public virtual Vector2 WindowMinSize => new(20.0f, 20.0f);
    public virtual Vector2 WindowTitleAlign => new(0.5f, 0.5f);
    public virtual ImGuiDir WindowMenuButtonPosition => ImGuiDir.None;
    public virtual float ChildRounding => 4.0f;
    public virtual float ChildBorderSize => 1.0f;
    public virtual float PopupRounding => 4.0f;
    public virtual float PopupBorderSize => 1.0f;
    public virtual Vector2 FramePadding => new(6.0f, 6.0f);
    public virtual float FrameRounding => 4.0f;
    public virtual float FrameBorderSize => 0.0f;
    public virtual Vector2 ItemSpacing => new(12.0f, 6.0f);
    public virtual Vector2 ItemInnerSpacing => new(6.0f, 3.0f);
    public virtual Vector2 CellPadding => new(12.0f, 6.0f);
    public virtual float IndentSpacing => 20.0f;
    public virtual float ColumnsMinSpacing => 6.0f;
    public virtual float ScrollbarSize => 12.0f;
    public virtual float ScrollbarRounding => 4.0f;
    public virtual float GrabMinSize => 12.0f;
    public virtual float GrabRounding => 4.0f;
    public virtual float TabRounding => 4.0f;
    public virtual float TabBorderSize => 0.0f;
    public virtual float TabMinWidthForCloseButton => 0.0f;
    public virtual ImGuiDir ColorButtonPosition => ImGuiDir.Right;
    public virtual Vector2 ButtonTextAlign => new(0.5f, 0.5f);
    public virtual Vector2 SelectableTextAlign => new(0.0f, 0.0f);
    public Vector4[] Colors = new Vector4[(int)ImGuiCol.COUNT];
    
    public ThemeDefinition(string name, bool isDefault) : base(Guid.NewGuid(), name, isDefault)
    {
    }

    public void Apply()
    {
        var style = ImGui.GetStyle();                                
                                                                   
        style.Alpha = Alpha;                                        
        style.DisabledAlpha = DisabledAlpha;                 
        style.WindowPadding = WindowPadding;             
        style.WindowRounding = WindowRounding;                               
        style.WindowBorderSize = WindowBorderSize;                             
        style.WindowMinSize = WindowMinSize;           
        style.WindowTitleAlign = WindowTitleAlign;          
        style.WindowMenuButtonPosition = WindowMenuButtonPosition;            
        style.ChildRounding = ChildRounding;                                
        style.ChildBorderSize = ChildBorderSize;                              
        style.PopupRounding = PopupRounding;                                
        style.PopupBorderSize = PopupBorderSize;                              
        style.FramePadding = FramePadding;              
        style.FrameRounding = FrameRounding;                                
        style.FrameBorderSize = FrameBorderSize;                              
        style.ItemSpacing = ItemSpacing;               
        style.ItemInnerSpacing = ItemInnerSpacing;          
        style.CellPadding = CellPadding;               
        style.IndentSpacing = IndentSpacing;                               
        style.ColumnsMinSpacing = ColumnsMinSpacing;                            
        style.ScrollbarSize = ScrollbarSize;                               
        style.ScrollbarRounding = ScrollbarRounding;                            
        style.GrabMinSize = GrabMinSize;                                 
        style.GrabRounding = GrabRounding;                                 
        style.TabRounding = TabRounding;                                  
        style.TabBorderSize = TabBorderSize;                                
        style.TabMinWidthForCloseButton = TabMinWidthForCloseButton;                    
        style.ColorButtonPosition = ColorButtonPosition;                
        style.ButtonTextAlign = ButtonTextAlign;           
        style.SelectableTextAlign = SelectableTextAlign;       
        
        style.Colors[(int)ImGuiCol.Text] = Colors[(int)ImGuiCol.Text];
        style.Colors[(int)ImGuiCol.TextDisabled] = Colors[(int)ImGuiCol.TextDisabled];
        style.Colors[(int)ImGuiCol.WindowBg] = Colors[(int)ImGuiCol.WindowBg];
        style.Colors[(int)ImGuiCol.ChildBg] = Colors[(int)ImGuiCol.ChildBg];
        style.Colors[(int)ImGuiCol.PopupBg] = Colors[(int)ImGuiCol.PopupBg];
        style.Colors[(int)ImGuiCol.Border] = Colors[(int)ImGuiCol.Border];
        style.Colors[(int)ImGuiCol.BorderShadow] = Colors[(int)ImGuiCol.BorderShadow];
        style.Colors[(int)ImGuiCol.FrameBg] = Colors[(int)ImGuiCol.FrameBg];
        style.Colors[(int)ImGuiCol.FrameBgHovered] = Colors[(int)ImGuiCol.FrameBgHovered];
        style.Colors[(int)ImGuiCol.FrameBgActive] = Colors[(int)ImGuiCol.FrameBgActive];
        style.Colors[(int)ImGuiCol.TitleBg] = Colors[(int)ImGuiCol.TitleBg];
        style.Colors[(int)ImGuiCol.TitleBgActive] = Colors[(int)ImGuiCol.TitleBgActive];
        style.Colors[(int)ImGuiCol.TitleBgCollapsed] = Colors[(int)ImGuiCol.TitleBgCollapsed];
        style.Colors[(int)ImGuiCol.MenuBarBg] = Colors[(int)ImGuiCol.MenuBarBg];
        style.Colors[(int)ImGuiCol.ScrollbarBg] = Colors[(int)ImGuiCol.ScrollbarBg];
        style.Colors[(int)ImGuiCol.ScrollbarGrab] = Colors[(int)ImGuiCol.ScrollbarGrab];
        style.Colors[(int)ImGuiCol.ScrollbarGrabHovered] = Colors[(int)ImGuiCol.ScrollbarGrabHovered];
        style.Colors[(int)ImGuiCol.ScrollbarGrabActive] = Colors[(int)ImGuiCol.ScrollbarGrabActive];
        style.Colors[(int)ImGuiCol.CheckMark] = Colors[(int)ImGuiCol.CheckMark];
        style.Colors[(int)ImGuiCol.SliderGrab] = Colors[(int)ImGuiCol.SliderGrab];
        style.Colors[(int)ImGuiCol.SliderGrabActive] = Colors[(int)ImGuiCol.SliderGrabActive];
        style.Colors[(int)ImGuiCol.Button] = Colors[(int)ImGuiCol.Button];
        style.Colors[(int)ImGuiCol.ButtonHovered] = Colors[(int)ImGuiCol.ButtonHovered];
        style.Colors[(int)ImGuiCol.ButtonActive] = Colors[(int)ImGuiCol.ButtonActive];
        style.Colors[(int)ImGuiCol.Header] = Colors[(int)ImGuiCol.Header];
        style.Colors[(int)ImGuiCol.HeaderHovered] = Colors[(int)ImGuiCol.HeaderHovered];
        style.Colors[(int)ImGuiCol.HeaderActive] = Colors[(int)ImGuiCol.HeaderActive];
        style.Colors[(int)ImGuiCol.Separator] = Colors[(int)ImGuiCol.Separator];
        style.Colors[(int)ImGuiCol.SeparatorHovered] = Colors[(int)ImGuiCol.SeparatorHovered];
        style.Colors[(int)ImGuiCol.SeparatorActive] = Colors[(int)ImGuiCol.SeparatorActive];
        style.Colors[(int)ImGuiCol.ResizeGrip] = Colors[(int)ImGuiCol.ResizeGrip];
        style.Colors[(int)ImGuiCol.ResizeGripHovered] = Colors[(int)ImGuiCol.ResizeGripHovered];
        style.Colors[(int)ImGuiCol.ResizeGripActive] = Colors[(int)ImGuiCol.ResizeGripActive];
        style.Colors[(int)ImGuiCol.Tab] = Colors[(int)ImGuiCol.Tab];
        style.Colors[(int)ImGuiCol.TabHovered] = Colors[(int)ImGuiCol.TabHovered];
        style.Colors[(int)ImGuiCol.TabActive] = Colors[(int)ImGuiCol.TabActive];
        style.Colors[(int)ImGuiCol.TabUnfocused] = Colors[(int)ImGuiCol.TabUnfocused];
        style.Colors[(int)ImGuiCol.TabUnfocusedActive] = Colors[(int)ImGuiCol.TabUnfocusedActive];
        style.Colors[(int)ImGuiCol.DockingPreview] = Colors[(int)ImGuiCol.DockingPreview];
        style.Colors[(int)ImGuiCol.DockingEmptyBg] = Colors[(int)ImGuiCol.DockingEmptyBg];
        style.Colors[(int)ImGuiCol.PlotLines] = Colors[(int)ImGuiCol.PlotLines];
        style.Colors[(int)ImGuiCol.PlotLinesHovered] = Colors[(int)ImGuiCol.PlotLinesHovered];
        style.Colors[(int)ImGuiCol.PlotHistogram] = Colors[(int)ImGuiCol.PlotHistogram];
        style.Colors[(int)ImGuiCol.PlotHistogramHovered] = Colors[(int)ImGuiCol.PlotHistogramHovered];
        style.Colors[(int)ImGuiCol.TableHeaderBg] = Colors[(int)ImGuiCol.TableHeaderBg];
        style.Colors[(int)ImGuiCol.TableBorderStrong] = Colors[(int)ImGuiCol.TableBorderStrong];
        style.Colors[(int)ImGuiCol.TableBorderLight] = Colors[(int)ImGuiCol.TableBorderLight];
        style.Colors[(int)ImGuiCol.TableRowBg] = Colors[(int)ImGuiCol.TableRowBg];
        style.Colors[(int)ImGuiCol.TableRowBgAlt] = Colors[(int)ImGuiCol.TableRowBgAlt];
        style.Colors[(int)ImGuiCol.TextSelectedBg] = Colors[(int)ImGuiCol.TextSelectedBg];
        style.Colors[(int)ImGuiCol.DragDropTarget] = Colors[(int)ImGuiCol.DragDropTarget];
        style.Colors[(int)ImGuiCol.NavHighlight] = Colors[(int)ImGuiCol.NavHighlight];
        style.Colors[(int)ImGuiCol.NavWindowingHighlight] = Colors[(int)ImGuiCol.NavWindowingHighlight];
        style.Colors[(int)ImGuiCol.NavWindowingDimBg] = Colors[(int)ImGuiCol.NavWindowingDimBg];
        style.Colors[(int)ImGuiCol.ModalWindowDimBg] = Colors[(int)ImGuiCol.ModalWindowDimBg];
    }
}
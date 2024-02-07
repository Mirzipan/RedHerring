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
    }
}
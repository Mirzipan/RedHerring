using IconFonts;
using static ImGuiNET.ImGui;

namespace RedHerring.ImGui;

public static class IconButton
{
    public static bool Regular(string icon)
    {
        PushFont(Font.FASolid);
        if (Button(icon))
        {
            PopFont();
            return true;
        }
        
        PopFont();
        return false;
    }
    
    public static bool Small(string icon)
    {
        PushFont(Font.FASolid);
        if (SmallButton(icon))
        {
            PopFont();
            return true;
        }
        
        PopFont();
        return false;
    }
    
    public static bool Add(ButtonSize size) => SizedButton(size)(FontAwesome6.Plus);

    public static bool Remove(ButtonSize size) => SizedButton(size)(FontAwesome6.Trash);

    private static Func<string, bool> SizedButton(ButtonSize size)
    {
        return size switch {
            ButtonSize.Regular => Regular,
            ButtonSize.Small => Small,
            _ => Regular,
        };

    }
}
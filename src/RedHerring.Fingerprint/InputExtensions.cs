using RedHerring.Fingerprint.Shortcuts;

namespace RedHerring.Fingerprint;

public static class InputExtensions
{
    public static Input AddKeyboardBinding(this Input @this, string name, Key key, Modifiers modifiers = Modifiers.None)
    {
        Shortcut shortcut;
        
        if (modifiers == Modifiers.None)
        {
            shortcut = new KeyboardShortcut(key);
        }
        else
        {
            shortcut = ModifierShortcut(modifiers);
            ((CompositeShortcut)shortcut).Add(new KeyboardShortcut(key));
        }

        @this.Bindings?.Add(new ShortcutBinding(name, shortcut));
        return @this;
    }

    public static Input AddMouseBinding(this Input @this, string name, MouseButton button,
        Modifiers modifiers = Modifiers.None)
    {
        Shortcut shortcut;
        
        if (modifiers == Modifiers.None)
        {
            shortcut = new MouseButtonShortcut(button);
        }
        else
        {
            shortcut = ModifierShortcut(modifiers);
            ((CompositeShortcut)shortcut).Add(new MouseButtonShortcut(button));
        }

        @this.Bindings?.Add(new ShortcutBinding(name, shortcut));
        return @this;
    }

    public static Input AddMouseBinding(this Input @this, string name, MouseAxis axis,
        Modifiers modifiers = Modifiers.None)
    {
        Shortcut shortcut;
        
        if (modifiers == Modifiers.None)
        {
            shortcut = new MouseAxisShortcut(axis);
        }
        else
        {
            shortcut = ModifierShortcut(modifiers);
            ((CompositeShortcut)shortcut).Add(new MouseAxisShortcut(axis));
        }

        @this.Bindings?.Add(new ShortcutBinding(name, shortcut));
        return @this;
    }

    public static Input AddGamepadBinding(this Input @this, string name, GamepadButton button,
        Modifiers modifiers = Modifiers.None)
    {
        Shortcut shortcut;
        
        if (modifiers == Modifiers.None)
        {
            shortcut = new GamepadButtonShortcut(button);
        }
        else
        {
            shortcut = ModifierShortcut(modifiers);
            ((CompositeShortcut)shortcut).Add(new GamepadButtonShortcut(button));
        }

        @this.Bindings?.Add(new ShortcutBinding(name, shortcut));
        return @this;
    }

    public static Input AddGamepadBinding(this Input @this, string name, GamepadAxis axis,
        Modifiers modifiers = Modifiers.None)
    {
        Shortcut shortcut;
        
        if (modifiers == Modifiers.None)
        {
            shortcut = new GamepadAxisShortcut(axis);
        }
        else
        {
            shortcut = ModifierShortcut(modifiers);
            ((CompositeShortcut)shortcut).Add(new GamepadAxisShortcut(axis));
        }

        @this.Bindings?.Add(new ShortcutBinding(name, shortcut));
        return @this;
    }

    private static CompositeShortcut ModifierShortcut(Modifiers modifiers)
    {
        var result = new CompositeShortcut();
        if ((modifiers & Modifiers.Alt) != 0)
        {
            result.Add(new KeyboardShortcut(Key.AltLeft));
        }
        
        if ((modifiers & Modifiers.Control) != 0)
        {
            result.Add(new KeyboardShortcut(Key.ControlLeft));
        }
        
        if ((modifiers & Modifiers.Shift) != 0)
        {
            result.Add(new KeyboardShortcut(Key.ShiftLeft));
        }
        
        if ((modifiers & Modifiers.Super) != 0)
        {
            result.Add(new KeyboardShortcut(Key.SuperLeft));
        }

        return result;
    }
}
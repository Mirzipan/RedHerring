using RedHerring.Fingerprint.Shortcuts;

namespace RedHerring.Fingerprint;

public static class InputExtensions
{
    public static Input AddKeyboardBinding(this Input @this, string name, Key key, Modifiers modifiers = Modifiers.None)
    {
        Shortcut shortcut = new KeyboardShortcut(key, modifiers);
        @this.Bindings?.Add(new ShortcutBinding(name, shortcut));
        return @this;
    }

    public static Input AddMouseBinding(this Input @this, string name, MouseButton button,
        Modifiers modifiers = Modifiers.None)
    {
        Shortcut shortcut = new MouseButtonShortcut(button, modifiers);
        @this.Bindings?.Add(new ShortcutBinding(name, shortcut));
        return @this;
    }

    public static Input AddMouseBinding(this Input @this, string name, MouseAxis axis,
        Modifiers modifiers = Modifiers.None)
    {
        Shortcut shortcut = new MouseAxisShortcut(axis, modifiers);
        @this.Bindings?.Add(new ShortcutBinding(name, shortcut));
        return @this;
    }

    public static Input AddGamepadBinding(this Input @this, string name, GamepadButton button,
        Modifiers modifiers = Modifiers.None)
    {
        Shortcut shortcut = new GamepadButtonShortcut(button, modifiers);
        @this.Bindings?.Add(new ShortcutBinding(name, shortcut));
        return @this;
    }

    public static Input AddGamepadBinding(this Input @this, string name, GamepadAxis axis,
        Modifiers modifiers = Modifiers.None)
    {
        Shortcut shortcut = new GamepadAxisShortcut(axis, modifiers);
        @this.Bindings?.Add(new ShortcutBinding(name, shortcut));
        return @this;
    }
}
using System.Runtime.CompilerServices;
using RedHerring.Fingerprint.Shortcuts;

namespace RedHerring.Fingerprint;

public static class InputExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static InputState State(this Input @this) => Interaction.State(@this);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsUp(this Input @this) => Interaction.IsUp(@this);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsPressed(this Input @this) => Interaction.IsPressed(@this);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsDown(this Input @this) => Interaction.IsDown(@this);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsReleased(this Input @this) => Interaction.IsReleased(@this);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float AnalogValue(this Input @this) => Interaction.AnalogValue(@this);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool AreDown(this Modifiers @this) => Interaction.AreDown(@this);
    
    public static InteractionContext AddKeyboardBinding(this InteractionContext @this, string name, Key key, Modifiers modifiers = Modifiers.None)
    {
        Shortcut shortcut = new KeyboardShortcut(key, modifiers);
        @this.Bindings?.Add(new ShortcutBinding(name, shortcut));
        return @this;
    }

    public static InteractionContext AddMouseBinding(this InteractionContext @this, string name, MouseButton button,
        Modifiers modifiers = Modifiers.None)
    {
        Shortcut shortcut = new MouseButtonShortcut(button, modifiers);
        @this.Bindings?.Add(new ShortcutBinding(name, shortcut));
        return @this;
    }

    public static InteractionContext AddMouseBinding(this InteractionContext @this, string name, MouseAxis axis,
        Modifiers modifiers = Modifiers.None)
    {
        Shortcut shortcut = new MouseAxisShortcut(axis, modifiers);
        @this.Bindings?.Add(new ShortcutBinding(name, shortcut));
        return @this;
    }

    public static InteractionContext AddGamepadBinding(this InteractionContext @this, string name, GamepadButton button,
        Modifiers modifiers = Modifiers.None)
    {
        Shortcut shortcut = new GamepadButtonShortcut(button, modifiers);
        @this.Bindings?.Add(new ShortcutBinding(name, shortcut));
        return @this;
    }

    public static InteractionContext AddGamepadBinding(this InteractionContext @this, string name, GamepadAxis axis,
        Modifiers modifiers = Modifiers.None)
    {
        Shortcut shortcut = new GamepadAxisShortcut(axis, modifiers);
        @this.Bindings?.Add(new ShortcutBinding(name, shortcut));
        return @this;
    }
}
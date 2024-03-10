using System.Runtime.CompilerServices;

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
    public static bool AreDown(this Modifier @this) => Interaction.AreDown(@this);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static InputState State(this Input @this, InteractionContext context) => context.State(@this);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsUp(this Input @this, InteractionContext context) => context.IsUp(@this);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsPressed(this Input @this, InteractionContext context) => context.IsPressed(@this);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsDown(this Input @this, InteractionContext context) => context.IsDown(@this);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsReleased(this Input @this, InteractionContext context) => context.IsReleased(@this);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float AnalogValue(this Input @this, InteractionContext context) => context.AnalogValue(@this);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool AreDown(this Modifier @this, InteractionContext context) => context.AreDown(@this);
    
    public static InteractionContext AddBinding(this InteractionContext @this, string name, Input input, Modifier modifiers = Modifier.None)
    {
        Shortcut oldShortcut = new Shortcut(input, modifiers);
        @this.Bindings?.Add(new ShortcutBinding(name, oldShortcut));
        return @this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Source ToSource(this Input @this)
    {
        return @this switch
        {
            > Input.KeysOffset => Source.Keyboard,
            > Input.MouseAxesOffset => Source.MouseAxis,
            > Input.MouseButtonsOffset => Source.MouseButton,
            > Input.GamepadAxesOffset => Source.GamepadAxis,
            > Input.GamepadButtonsOffset => Source.MouseButton,
            _ => Source.Keyboard,
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Modifier ToModifiers(this Input @this)
    {
        return @this switch
        {
            Input.AltLeft => Modifier.Alt,
            Input.AltRight => Modifier.Alt,
            Input.ControlLeft => Modifier.Control,
            Input.ControlRight => Modifier.Control,
            Input.ShiftLeft => Modifier.Shift,
            Input.ShiftRight => Modifier.Shift,
            Input.SuperLeft => Modifier.Super,
            Input.SuperRight => Modifier.Super,
            _ => Modifier.None,
        };
    }
}
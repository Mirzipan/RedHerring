namespace RedHerring.Fingerprint.States;

public static class StateExtensions
{
    public static bool IsAltDown(this KeyboardState @this) => (@this.Modifiers & Modifiers.Alt) != 0;
    public static bool IsControlDown(this KeyboardState @this) => (@this.Modifiers & Modifiers.Control) != 0;
    public static bool IsShiftDown(this KeyboardState @this) => (@this.Modifiers & Modifiers.Shift) != 0;
    public static bool IsSuperDown(this KeyboardState @this) => (@this.Modifiers & Modifiers.Super) != 0;
}
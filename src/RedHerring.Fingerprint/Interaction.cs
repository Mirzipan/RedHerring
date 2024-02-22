using System.Runtime.CompilerServices;
using RedHerring.Alexandria.Disposables;
using Silk.NET.Windowing;

namespace RedHerring.Fingerprint;

public static class Interaction
{
    private static InteractionContext? _context;

    #region Lifecycle
    
    public static InteractionContext CreateContext(IView? view)
    {
        var previous = CurrentContext();
        InteractionContext context = view is not null ? new SilkInteractionContext(view) : new NullInteractionContext();
        CurrentContext(previous ?? context);
        
        return context;
    }

    public static void DestroyContext(InteractionContext? context = null)
    {
        var previous = CurrentContext();
        if (context is null)
        {
            context = previous;
        }

        CurrentContext(context != previous ? previous : null);
        context.TryDispose();
    }

    public static InteractionContext? CurrentContext() => _context;

    public static void CurrentContext(InteractionContext? context)
    {
        _context = context;
    }

    #endregion Lifecycle

    #region Queries

    public static InputState State(Input input) => InputState.Up;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsUp(Input input) => !IsDown(input);
    public static bool IsPressed(Input input) => false;
    public static bool IsDown(Input input) => false;
    public static bool IsReleased(Input input) => false;

    public static float AnalogValue(Input input) => 0.00f;

    public static Modifiers Modifiers() => Fingerprint.Modifiers.None;
    public static bool AreDown(Modifiers modifiers) => false;

    public static bool Any() => false;
    public static bool AnyKeyboardKey() => false;
    public static bool AnyMouseButton() => false;
    public static bool AnyMouseAxis() => false;
    public static bool AnyGamepadButton() => false;
    public static bool AnyGamepadAxis() => false;

    public static void Pressed(List<Input> result)
    {
    }
    
    public static void Down(List<Input> result)
    {
    }
    
    public static void Released(List<Input> result)
    {
    }
    
    public static void Characters(List<char> result)
    {
    }

    #endregion Queries
}
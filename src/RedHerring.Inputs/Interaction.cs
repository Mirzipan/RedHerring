using System.Numerics;
using RedHerring.Alexandria.Disposables;
using RedHerring.Inputs.Layers;
using Silk.NET.Windowing;

namespace RedHerring.Inputs;

public static class Interaction
{
    private static InputDevices _devices = new NullDevices();
    private static InteractionContext? _context;
    private static Processor _processor = new();

    #region Lifecycle

    public static void Init(IView? view)
    {
        if (view is not null)
        {
            _devices = new SilkDevices(view);
        }
        
        _devices.InputChanged += OnInputChanged;
        _devices.CharacterTyped += OnCharacterTyped;
        
        CreateContext();
    }

    public static void NextFrame()
    {
        _devices.NextFrame();

        if (_context is not null)
        {
            _processor.NextFrame(_context);
            _context.NextFrame();
        }
    }
    
    public static InteractionContext CreateContext()
    {
        var previous = CurrentContext();
        InteractionContext context = new InteractionContext();
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

    public static Vector2 MousePosition => _context?.MousePosition ?? Vector2.Zero;
    public static Vector2 MousePositionDelta => _context?.MousePositionDelta ?? Vector2.Zero;
    public static Vector2 MouseWheelDelta = _context?.MouseWheelDelta ?? Vector2.Zero;
    
    public static InputState State(Input input) => InputState.Up;
    public static bool IsPressed(Input input) => _context?.IsPressed(input) ?? false;
    public static bool IsDown(Input input) => _context?.IsDown(input) ?? false;
    public static bool IsReleased(Input input) => _context?.IsReleased(input) ?? false;

    public static float AnalogValue(Input input) => _context?.AnalogValue(input) ?? 0.00f;

    public static Modifier Modifiers() => Modifier.None;
    public static bool AreDown(Modifier modifiers) => _context?.AreDown(modifiers) ?? false;
    public static bool Any() => _context?.Any() ?? false;
    public static bool AnyKeyboardKey() =>  _context?.AnyKeyboardKey() ?? false;
    public static bool AnyMouseButton() =>  _context?.AnyMouseButton() ?? false;
    public static bool AnyMouseAxis() =>  _context?.AnyMouseAxis() ?? false;
    public static bool AnyGamepadButton() =>  _context?.AnyGamepadButton() ?? false;
    public static bool AnyGamepadAxis() =>  _context?.AnyGamepadAxis() ?? false;

    public static void Pressed(List<Input> result) => _context?.Pressed(result);
    public static void Down(List<Input> result) => _context?.Down(result);
    public static void Released(List<Input> result) => _context?.Released(result);
    public static void Characters(List<char> result) => _context?.Characters(result);

    #endregion Queries

    #region Manipulation

    public static void Cursor(CursorKind cursor)
    {
        _devices.Cursor(cursor);
    }

    private static void OnInputChanged(InputChanged evt)
    {
        _context?.OnInputChanged(evt);
    }

    private static void OnCharacterTyped(int deviceId, char character)
    {
        _context?.OnCharacterTyped(deviceId, character);
    }

    #endregion Manipulation
}
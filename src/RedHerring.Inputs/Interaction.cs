using System.Numerics;
using RedHerring.Alexandria.Disposables;
using RedHerring.Inputs.Layers;

namespace RedHerring.Inputs;

public static class Interaction
{
    private static InputDevices _devices = new NullDevices();
    private static InteractionContext _context = null!;
    private static Processor _processor = new();

    #region Lifecycle

    public static InteractionContext Init(InputDevices devices)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (_devices is not null)
        {
            _devices.InputChanged -= OnInputChanged;
            _devices.CharacterTyped -= OnCharacterTyped;
        }

        _devices = devices;
        _devices.InputChanged += OnInputChanged;
        _devices.CharacterTyped += OnCharacterTyped;
        
        return CreateContext();
    }

    public static void NextFrame()
    {
        _devices.NextFrame();

        _processor.NextFrame(_context);
        _context.NextFrame();
    }
    
    public static InteractionContext CreateContext()
    {
        var previous = CurrentContext();
        InteractionContext context = new InteractionContext();
        // ReSharper disable once NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract
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

        CurrentContext(context != previous ? previous : new InteractionContext());
        context.TryDispose();
    }

    public static InteractionContext CurrentContext() => _context;

    public static void CurrentContext(InteractionContext context)
    {
        _context = context;
    }

    #endregion Lifecycle

    #region Queries

    public static string Clipboard
    {
        get => _devices.Clipboard();
        set => _devices.Clipboard(value);
    }
    
    public static Vector2 MousePosition => _context.MousePosition;
    public static Vector2 MousePositionDelta => _context.MousePositionDelta;
    public static Vector2 MouseWheelDelta => _context.MouseWheelDelta;
    
    public static InputState State(Input input) => InputState.Up;
    public static bool IsPressed(Input input) => _context.IsPressed(input);
    public static bool IsDown(Input input) => _context.IsDown(input);
    public static bool IsReleased(Input input) => _context.IsReleased(input);

    public static float AnalogValue(Input input) => _context.AnalogValue(input);

    public static Modifier Modifiers() => Modifier.None;
    public static bool AreDown(Modifier modifiers) => _context.AreDown(modifiers);
    public static bool Any() => _context.Any();
    public static bool AnyKeyboardKey() =>  _context.AnyKeyboardKey();
    public static bool AnyMouseButton() =>  _context.AnyMouseButton();
    public static bool AnyMouseAxis() =>  _context.AnyMouseAxis();
    public static bool AnyGamepadButton() =>  _context.AnyGamepadButton();
    public static bool AnyGamepadAxis() =>  _context.AnyGamepadAxis();

    public static void Pressed(List<Input> result) => _context.Pressed(result);
    public static void Down(List<Input> result) => _context.Down(result);
    public static void Released(List<Input> result) => _context.Released(result);
    public static void Characters(List<char> result) => _context.Characters(result);

    #endregion Queries

    #region Manipulation

    public static void Cursor(CursorKind cursor)
    {
        _devices.Cursor(cursor);
    }

    private static void OnInputChanged(InputChanged evt)
    {
        _context.OnInputChanged(evt);
    }

    private static void OnCharacterTyped(int deviceId, char character)
    {
        _context.OnCharacterTyped(deviceId, character);
    }

    #endregion Manipulation
}
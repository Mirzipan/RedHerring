using System.Numerics;
using RedHerring.Fingerprint.Layers;
using RedHerring.Fingerprint.Shortcuts;

namespace RedHerring.Fingerprint;

public sealed class InteractionContext : IDisposable
{
    private const int DefaultCapacity = 64;
    
    private readonly List<Input> _pressed = new(DefaultCapacity);
    private readonly List<Input> _down = new(DefaultCapacity);
    private readonly List<Input> _released = new(DefaultCapacity);
    private readonly List<float> _analogValues = new(DefaultCapacity);
    
    private readonly List<char> _chars = new(DefaultCapacity);

    private readonly List<InputEvent> _events = new(DefaultCapacity);

    private readonly Dictionary<string, InputState> _actions = new(DefaultCapacity);
    
    private Modifier _modifiers = Modifier.None;
    private Source _source = Source.None;
    
    private Vector2 _previousMousePosition;
    private Vector2 _currentMousePosition;
    private Vector2 _currentMouseWheelDelta;
    
    private bool _isDebugging;

    public bool IsDebugging => _isDebugging;
    
    public ShortcutBindings? Bindings { get; set; }
    public InputLayers Layers { get; }
    
    #region Lifecycle

    internal InteractionContext()
    {
        Layers = new InputLayers();
        Bindings = new ShortcutBindings();
    }

    internal void NextFrame()
    {
        _previousMousePosition = _currentMousePosition;
        _currentMousePosition = Vector2.Zero;
        _currentMouseWheelDelta = Vector2.Zero;
        
        _events.Clear();
        
        _pressed.Clear();
        _released.Clear();
        _chars.Clear();
        
        _actions.Clear();
    }

    public void Dispose()
    {
    }

    #endregion Lifecycle

    #region Public

    public void EnableDebug()
    {
        _isDebugging = true;
    }

    public void DisableDebug()
    {
        _isDebugging = false;
    }
    
    #endregion Public
    
    #region Queries

    public Vector2 MousePosition => _currentMousePosition;
    public Vector2 MousePositionDelta => _currentMousePosition - _previousMousePosition;
    public Vector2 MouseWheelDelta => _currentMouseWheelDelta;
    
    public InputState State(Input input)
    {
        if (_down.Contains(input))
        {
            return _pressed.Contains(input) ? InputState.Pressed | InputState.Down : InputState.Down;
        }

        return _released.Contains(input) ? InputState.Released : InputState.Up;
    }
    
    public bool IsPressed(Input input) => _pressed.Contains(input);
    public bool IsDown(Input input) => _down.Contains(input);
    public bool IsReleased(Input input) => _released.Contains(input);

    public float AnalogValue(Input input)
    {
        int index = _down.IndexOf(input);
        return index >= 0 ? _analogValues[index] : 0.00f;
    }
    
    public InputState State(Shortcut shortcut)
    {
        return shortcut.Negative == Input.Unknown && AreDown(shortcut.Modifiers) ? State(shortcut.Positive) : InputState.Up;
    }
    public bool IsPressed(Shortcut shortcut)
    {
        return shortcut.Negative == Input.Unknown && AreDown(shortcut.Modifiers) && IsPressed(shortcut.Positive);
    }

    public bool IsDown(Shortcut shortcut)
    {
        return shortcut.Negative == Input.Unknown && AreDown(shortcut.Modifiers) && IsDown(shortcut.Positive);
    }

    public bool IsReleased(Shortcut shortcut)
    {
        return shortcut.Negative == Input.Unknown && AreDown(shortcut.Modifiers) && IsReleased(shortcut.Positive);
    }

    public float AnalogValue(Shortcut shortcut)
    {
        return AreDown(shortcut.Modifiers) ? AnalogValue(shortcut.Positive) - AnalogValue(shortcut.Negative) : 0f;
    }

    public bool IsPressed(string action)
    {
        return _actions.TryGetValue(action, out var state) && (state & InputState.Pressed) != 0;
    }

    public bool IsDown(string action)
    {
        return _actions.TryGetValue(action, out var state) && (state & InputState.Down) != 0;
    }

    public bool IsReleased(string action)
    {
        return _actions.TryGetValue(action, out var state) && (state & InputState.Released) != 0;
    }

    public bool AnyAction() => _actions.Count > 0;

    public void ActionsDown(IList<string> actions)
    {
        foreach (var pair in _actions)
        {
            actions.Add(pair.Key);
        }
    }

    public Modifier Modifiers() => _modifiers;

    public bool AreDown(Modifier modifiers) => (modifiers & _modifiers) == modifiers;

    public bool Any() => _source != Source.None;
    public bool AnyKeyboardKey() => (_source & Source.Keyboard) != 0;
    public bool AnyMouseButton() => (_source & Source.MouseButton) != 0;
    public bool AnyMouseAxis() => (_source & Source.MouseAxis) != 0;
    public bool AnyGamepadButton() => (_source & Source.GamepadButton) != 0;
    public bool AnyGamepadAxis() => (_source & Source.GamepadAxis) != 0;

    public void Pressed(List<Input> result) => result.AddRange(_pressed);
    public void Down(List<Input> result) => result.AddRange(_down);
    public void Released(List<Input> result) => result.AddRange(_released);
    public void Characters(List<char> result) => result.AddRange(_chars);
    public void PumpEvents(List<InputEvent> result) => result.AddRange(_events);

    #endregion Queries

    #region Private

    #endregion Private

    #region Internal

    internal void OnActionChanged(string action, InputState state)
    {
        _actions[action] = state;
    }

    internal void OnInputChanged(InputEvent evt)
    {
        _events.Add(evt);
        var source = evt.Input.ToSource();
        var modifiers = evt.Input.ToModifiers();
        
        if (evt.IsDown)
        {
            switch (evt.Input)
            {
                case Input.MouseX:
                    _currentMousePosition.X = evt.AnalogValue;
                    break;
                case Input.MouseY:
                    _currentMousePosition.Y = evt.AnalogValue;
                    break;
                case Input.MouseWheelX:
                    _currentMouseWheelDelta.X = evt.AnalogValue;
                    break;
                case Input.MouseWheelY:
                    _currentMouseWheelDelta.Y = evt.AnalogValue;
                    break;
            }
            
            _pressed.Add(evt.Input);
            
            _down.Add(evt.Input);
            _analogValues.Add(evt.AnalogValue);

            _source |= source;
            _modifiers |= modifiers;
        }
        else
        {
            int index = _down.IndexOf(evt.Input);
            if (index >= 0)
            {
                _down.RemoveAt(index);
                _analogValues.RemoveAt(index);
            }
            
            _released.Add(evt.Input);

            if (source != Source.None)
            {
                _source &= ~source;
            }
            
            if (modifiers != Modifier.None)
            {
                _modifiers &= ~modifiers;
            }
        }

        if (_isDebugging)
        {
            Console.WriteLine($"device={evt.DeviceId} input={evt.Input} down={evt.IsDown} analog={evt.AnalogValue:0.####} modifiers={_modifiers}");
        }
    }

    internal void OnCharacterTyped(int deviceId, char character)
    {
        _chars.Add(character);
    }

    #endregion Internal
}
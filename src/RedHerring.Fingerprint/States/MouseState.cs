using System.Numerics;
using RedHerring.Alexandria.Masks;
using RedHerring.Fingerprint.Events;
using Silk.NET.Input;
using SilkButton = Silk.NET.Input.MouseButton;

namespace RedHerring.Fingerprint.States;

public class MouseState : IDisposable
{
    internal static Action<string>? DebugPrint;
    
    private BitMask16 _down = new BitMask16(false);
    private BitMask16 _pressed = new BitMask16(false);
    private BitMask16 _released = new BitMask16(false);

    private Vector2 _position;
    private Vector2 _delta;
    private Vector2 _scrollWheel;

    public Vector2 Position => _position;
    public Vector2 Delta => _delta;
    public Vector2 ScrollWheel => _scrollWheel;

    private IMouse _mouse;
    private List<MouseButtonChanged> _buttonsChanged = new();

    public IMouse Mouse => _mouse;
    public string Name => _mouse.Name;
    public IEnumerable<MouseButtonChanged> ButtonsChanged => _buttonsChanged;

    #region Lifecycle

    internal MouseState(IMouse mouse)
    {
        _mouse = mouse;
        
        _position = Vector2.Zero;
        _delta = Vector2.Zero;
        _scrollWheel = Vector2.Zero;
        
        _mouse.MouseMove += OnMouseMove;
        _mouse.MouseDown += OnMouseDown;
        _mouse.MouseUp += OnMouseUp;
        _mouse.Scroll += OnMouseScroll;
    }

    internal void Reset()
    {
        _pressed.Set(false);
        _released.Set(false);
        _buttonsChanged.Clear();
        
        _delta = Vector2.Zero;
        _scrollWheel = Vector2.Zero;
    }

    public void Dispose()
    {
        _mouse.MouseMove -= OnMouseMove;
        _mouse.MouseDown -= OnMouseDown;
        _mouse.MouseUp -= OnMouseUp;
        _mouse.Scroll -= OnMouseScroll;

        _mouse = null!;
    }

    #endregion Lifecycle

    #region Queries

    public bool IsButtonUp(MouseButton button) => !_down.Get((int)button);
    public bool IsButtonPressed(MouseButton button) => _pressed.Get((int)button);
    public bool IsButtonDown(MouseButton button) => _down.Get((int)button);
    public bool IsButtonReleased(MouseButton button) => _released.Get((int)button);
    public bool IsAnyButtonDown() => _down != BitMask16.Empty;
    public void ButtonsDown(IList<MouseButton> buttons)
    {
        for (int i = 0; i < BitMask16.Count; i++)
        {
            if (_down[i])
            {
                buttons.Add((MouseButton)i);
            }
        }
    }

    public bool IsMoved(MouseAxis axis)
    {
        return axis switch
        {
            MouseAxis.None => false,
            MouseAxis.Horizontal => _delta.X != 0,
            MouseAxis.Vertical => _delta.Y != 0,
            MouseAxis.HorizontalDelta => _delta.X != 0,
            MouseAxis.VerticalDelta => _delta.Y != 0,
            MouseAxis.Wheel => _scrollWheel.Y != 0,
            MouseAxis.WheelUp => _scrollWheel.Y > float.Epsilon,
            MouseAxis.WheelDown => _scrollWheel.Y < -float.Epsilon,
            _ => false,
        };
    }

    public float Axis(MouseAxis axis)
    {
        return axis switch {
            MouseAxis.None => 0,
            MouseAxis.Horizontal => _position.X,
            MouseAxis.Vertical => _position.Y,
            MouseAxis.HorizontalDelta => _delta.X,
            MouseAxis.VerticalDelta => _delta.Y,
            MouseAxis.Wheel => _scrollWheel.Y,
            MouseAxis.WheelUp => _scrollWheel.Y,
            MouseAxis.WheelDown => _scrollWheel.Y,
            _ => 0,
        };

    }

    #endregion Queries

    #region Private

    private static MouseButton ConvertButton(SilkButton button)
    {
        return button switch
        {
            SilkButton.Unknown => MouseButton.Unknown,
            SilkButton.Left => MouseButton.Left,
            SilkButton.Right => MouseButton.Right,
            SilkButton.Middle => MouseButton.Middle,
            SilkButton.Button4 => MouseButton.Button4,
            SilkButton.Button5 => MouseButton.Button5,
            SilkButton.Button6 => MouseButton.Button6,
            SilkButton.Button7 => MouseButton.Button7,
            SilkButton.Button8 => MouseButton.Button8,
            SilkButton.Button9 => MouseButton.Button9,
            SilkButton.Button10 => MouseButton.Button10,
            SilkButton.Button11 => MouseButton.Button11,
            SilkButton.Button12 => MouseButton.Button12,
            _ => MouseButton.Unknown,
        };
    }

    #endregion Private

    #region Bindings

    private void OnMouseMove(IMouse mouse, Vector2 position)
    {
        var delta = position - _position;
        
        _delta += delta;
        _position = position;
        
        DebugPrint?.Invoke($"`{mouse.Name}` moved to `{position}`.");
    }

    private void OnMouseDown(IMouse mouse, SilkButton silkButton)
    {
        var button = ConvertButton(silkButton);
        _pressed[(int)button] = true;
        _down[(int)button] = true;
        
        _buttonsChanged.Add(new MouseButtonChanged(button, Modifiers.None, true));
            
        DebugPrint?.Invoke($"`{mouse.Name}` button `{button}` pressed.");
    }

    private void OnMouseUp(IMouse mouse, SilkButton silkButton)
    {
        var button = ConvertButton(silkButton);
        _down[(int)button] = false;
        _released[(int)button] = true;
        
        _buttonsChanged.Add(new MouseButtonChanged(button, Modifiers.None, false));
        
        DebugPrint?.Invoke($"`{mouse.Name}` button `{button}` released.");
    }

    private void OnMouseScroll(IMouse mouse, ScrollWheel scrollDelta)
    {
        _scrollWheel = new Vector2(scrollDelta.X, scrollDelta.Y);
        
        DebugPrint?.Invoke($"`{mouse.Name}` scrolled by `{scrollDelta}`.");
    }

    #endregion Bindings
}
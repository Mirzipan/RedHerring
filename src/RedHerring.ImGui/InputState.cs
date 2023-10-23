using System.Numerics;
using RedHerring.Alexandria.Masks;
using RedHerring.Fingerprint;
using Veldrid;
using MouseButton = RedHerring.Fingerprint.MouseButton;
using VKey = Veldrid.Key;
using VMouseButton = Veldrid.MouseButton;

namespace RedHerring.ImGui;

internal class InputState : InputSnapshot
{
    private static readonly List<MouseButton> _tmpButtons = new();
    
    private IInput _input;
    private readonly List<KeyEvent> _keyEvents = new();
    private readonly List<MouseEvent> _mouseEvents = new();
    private readonly List<char> _keyCharPresses = new();

    private BitMask8 _mouseButtons = new BitMask8(false);

    public IReadOnlyList<KeyEvent> KeyEvents => _keyEvents;
    public IReadOnlyList<MouseEvent> MouseEvents => _mouseEvents;
    public IReadOnlyList<char> KeyCharPresses => _keyCharPresses;
    public Vector2 MousePosition => _input.MousePosition;
    public float WheelDelta => _input.MouseWheelDelta;

    public InputState(IInput input)
    {
        _input = input;
    }

    public void Update()
    {
        // TODO: update key events
        _mouseEvents.Clear();
        CreateMouseEvents();
        
        UpdateMouseButtons();
    }

    public bool IsMouseDown(VMouseButton button) => _mouseButtons[(int)button];

    private void CreateMouseEvents()
    {
        if (_input.Mouse is null)
        {
            return;
        }
        
        const int maxButton = (int)MouseButton.Button5;
        for (int i = 0; i <= maxButton; i++)
        {
            if (_input.Mouse.IsButtonPressed((MouseButton)i))
            {
                _mouseEvents.Add(new MouseEvent((VMouseButton)i, true));
            }

            if (_input.Mouse.IsButtonReleased((MouseButton)i))
            {
                _mouseEvents.Add(new MouseEvent((VMouseButton)i, false));
            }
        }
    }

    private void UpdateMouseButtons()
    {
        _mouseButtons = BitMask8.Empty;
        _tmpButtons.Clear();

        if (_input.Mouse is null)
        {
            return;
        }

        _input.Mouse.GetButtonsDown(_tmpButtons);
        foreach (var button in _tmpButtons)
        {
            _mouseButtons[(int)button] = true;
        }
    }
    
    private static MouseButton Convert(VMouseButton button)
    {
        return button switch
        {
            VMouseButton.Left => MouseButton.Left,
            VMouseButton.Middle => MouseButton.Middle,
            VMouseButton.Right => MouseButton.Right,
            VMouseButton.Button1 => MouseButton.Button4,
            VMouseButton.Button2 => MouseButton.Button5,
            VMouseButton.Button3 => MouseButton.Button6,
            VMouseButton.Button4 => MouseButton.Button7,
            VMouseButton.Button5 => MouseButton.Button8,
            VMouseButton.Button6 => MouseButton.Button9,
            VMouseButton.Button7 => MouseButton.Button10,
            VMouseButton.Button8 => MouseButton.Button11,
            VMouseButton.Button9 => MouseButton.Button12,
            VMouseButton.LastButton => MouseButton.Button12,
            _ => MouseButton.Unknown,
        };
    }

}
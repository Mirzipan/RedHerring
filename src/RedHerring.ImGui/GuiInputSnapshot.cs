using System.Numerics;
using RedHerring.Fingerprint;
using Veldrid;
using MouseButton = RedHerring.Fingerprint.MouseButton;
using VMouseButton = Veldrid.MouseButton;

namespace RedHerring.ImGui;

public class GuiInputSnapshot : InputSnapshot
{
    private IInput _input;

    public IReadOnlyList<KeyEvent> KeyEvents { get; } = new List<KeyEvent>();
    public IReadOnlyList<MouseEvent> MouseEvents { get; } = new List<MouseEvent>();
    public IReadOnlyList<char> KeyCharPresses { get; } = new List<char>();
    public Vector2 MousePosition => _input.MousePosition;
    public float WheelDelta => _input.MouseWheelDelta;
    
    public GuiInputSnapshot(IInput input)
    {
        _input = input;
    }

    public void Update()
    {
        // TODO: update key events
    }
    
    public bool IsMouseDown(VMouseButton button) => _input.IsButtonDown(Convert(button));

    private static MouseButton Convert(VMouseButton button)
    {
        return button switch
        {
            VMouseButton.Left => MouseButton.Left,
            VMouseButton.Middle => MouseButton.Middle,
            VMouseButton.Right => MouseButton.Right,
            VMouseButton.Button1 => MouseButton.Left,
            VMouseButton.Button2 => MouseButton.Middle,
            VMouseButton.Button3 => MouseButton.Right,
            VMouseButton.Button4 => MouseButton.Button4,
            VMouseButton.Button5 => MouseButton.Button5,
            VMouseButton.Button6 => MouseButton.Button6,
            VMouseButton.Button7 => MouseButton.Button7,
            VMouseButton.Button8 => MouseButton.Button8,
            VMouseButton.Button9 => MouseButton.Button9,
            VMouseButton.LastButton => MouseButton.Button9,
            _ => MouseButton.Unknown,
        };
    }

}
using System.Numerics;
using RedHerring.Alexandria.Masks;
using Silk.NET.Input;

namespace RedHerring.Fingerprint;

internal struct MouseState
{
    private BitMask8 _buffer = new BitMask8(false);

    public Vector2 Position;
    public Vector2 ScrollWheel;

    public MouseState()
    {
        Position = Vector2.Zero;
        ScrollWheel = Vector2.Zero;
    }

    public bool IsButtonUp(MouseButton button) => !_buffer.Get((int)button);
    public bool IsButtonDown(MouseButton button) => _buffer.Get((int)button);
    public bool IsAnyButtonDown() => _buffer != BitMask8.Empty;

    public void SetButton(MouseButton button, bool isPressed)
    {
        _buffer[(int)button] = isPressed;
    }
}
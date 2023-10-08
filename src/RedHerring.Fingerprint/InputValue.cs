using Silk.NET.Input;

namespace RedHerring.Fingerprint;

public struct InputValue
{
    public InputSource Source;
    public int Code;

    public InputValue(Key key)
    {
        Source = InputSource.Keyboard;
        Code = (int)key;
    }

    public InputValue(MouseButton button)
    {
        Source = InputSource.MouseButton;
        Code = (int)button;
    }

    public InputValue(MouseAxis axis)
    {
        Source = InputSource.MouseAxis;
        Code = (int)axis;
    }

    public InputValue(ButtonName button)
    {
        Source = InputSource.ControllerButton;
        Code = (int)button;
    }

    public Key GetKey() => (Key)Code;
    public MouseButton GetMouseButton() => (MouseButton)Code;
    public MouseAxis GetMouseAxis() => (MouseAxis)Code;
    public ButtonName GetControllerButton() => (ButtonName)Code;
}
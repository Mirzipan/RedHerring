namespace RedHerring.Fingerprint.Shortcuts;

public sealed class GamepadAxisShortcut : Shortcut
{
    private InputCode _code;
    public GamepadAxis Axis => _code.GamepadAxis;

    public GamepadAxisShortcut(GamepadAxis axis)
    {
        _code = new InputCode(axis);
    }

    public void InputCodes(IList<InputCode> result)
    {
        result.Add(_code);
    }

    public float Value(Input input) => input.Axis(_code.GamepadAxis);

    public bool IsUp(Input input) => false;

    public bool IsPressed(Input input) => false;

    public bool IsDown(Input input) => false;

    public bool IsReleased(Input input) => false;
}
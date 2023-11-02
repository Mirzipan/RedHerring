namespace RedHerring.Fingerprint.Shortcuts;

public sealed class MouseAxisShortcut : Shortcut
{
    private InputCode _code;
    public MouseAxis Axis => _code.MouseAxis;
    
    public MouseAxisShortcut(MouseAxis axis) => _code = new InputCode(axis);

    public void InputCodes(IList<InputCode> result) => result.Add(_code);

    public float Value(Input input) => input.Axis(_code.MouseAxis);

    public bool IsUp(Input input) => false;

    public bool IsPressed(Input input) => false;

    public bool IsDown(Input input) => false;

    public bool IsReleased(Input input) => false;
}
namespace RedHerring.Fingerprint.Shortcuts;

public sealed class MouseButtonShortcut : Shortcut
{
    private InputCode _code;
    public MouseButton Button => _code.MouseButton;
    
    public MouseButtonShortcut(MouseButton button) => _code = new InputCode(button);

    public void InputCodes(IList<InputCode> result) => result.Add(_code);

    public float Value(Input input) => IsDown(input) ? 1f : 0f;

    public bool IsUp(Input input) => input.IsButtonUp(_code.MouseButton);

    public bool IsPressed(Input input) => input.IsButtonPressed(_code.MouseButton);

    public bool IsDown(Input input) => input.IsButtonDown(_code.MouseButton);

    public bool IsReleased(Input input) => input.IsButtonReleased(_code.MouseButton);
}
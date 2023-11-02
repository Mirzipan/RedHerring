namespace RedHerring.Fingerprint.Shortcuts;

public sealed class GamepadButtonShortcut : Shortcut
{
    private InputCode _code;
    public GamepadButton Button => _code.GamepadButton;
    
    public GamepadButtonShortcut(GamepadButton button) => _code = new InputCode(button);

    public void InputCodes(IList<InputCode> result) => result.Add(_code);

    public float Value(Input input) => IsDown(input) ? 1f : 0f;

    public bool IsUp(Input input) => input.IsButtonUp(_code.GamepadButton);

    public bool IsPressed(Input input) => input.IsButtonPressed(_code.GamepadButton);

    public bool IsDown(Input input) => input.IsButtonDown(_code.GamepadButton);

    public bool IsReleased(Input input) => input.IsButtonReleased(_code.GamepadButton);
}
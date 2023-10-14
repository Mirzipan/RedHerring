namespace RedHerring.Fingerprint.Shortcuts;

public class GamepadButtonShortcut : AShortcut
{
    public GamepadButtonShortcut(GamepadButton button) : base(InputSource.GamepadButton, (int)button)
    {
    }
    
    public override float GetValue(Input input) => IsDown(input) ? 1f : 0f;

    public override bool IsUp(Input input) => input.IsButtonUp(Code.GamepadButton);

    public override bool IsPressed(Input input) => input.IsButtonPressed(Code.GamepadButton);

    public override bool IsDown(Input input) => input.IsButtonDown(Code.GamepadButton);

    public override bool IsReleased(Input input) => input.IsButtonReleased(Code.GamepadButton);
}
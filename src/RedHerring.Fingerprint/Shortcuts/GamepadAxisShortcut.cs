namespace RedHerring.Fingerprint.Shortcuts;

public class GamepadAxisShortcut : AShortcut
{
    public GamepadAxisShortcut(GamepadAxis axis) : base(InputSource.GamepadAxis, (int)axis)
    {
    }

    public override float GetValue(Input input) => input.Axis(Code.GamepadAxis);

    public override bool IsUp(Input input) => false;

    public override bool IsPressed(Input input) => false;

    public override bool IsDown(Input input) => false;

    public override bool IsReleased(Input input) => false;
}
namespace RedHerring.Fingerprint.Shortcuts;

public class MouseAxisShortcut : AShortcut
{
    public MouseAxisShortcut(MouseAxis axis) : base(InputSource.MouseAxis, (int)axis)
    {
    }

    public override float GetValue(Input input) => input.GetAxis(Code.MouseAxis);

    public override bool IsUp(Input input) => false;

    public override bool IsPressed(Input input) => false;

    public override bool IsDown(Input input) => false;

    public override bool IsReleased(Input input) => false;
}
namespace RedHerring.Fingerprint.Shortcuts;

public class MouseButtonShortcut : AShortcut
{
    public MouseButtonShortcut(MouseButton button) : base(InputSource.MouseButton, (int)button)
    {
    }

    public override float GetValue(Input input) => IsDown(input) ? 1f : 0f;

    public override bool IsUp(Input input) => input.IsButtonUp(Code.MouseButton);

    public override bool IsPressed(Input input) => input.IsButtonPressed(Code.MouseButton);

    public override bool IsDown(Input input) => input.IsButtonDown(Code.MouseButton);

    public override bool IsReleased(Input input) => input.IsButtonReleased(Code.MouseButton);
}
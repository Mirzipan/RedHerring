namespace RedHerring.Fingerprint.Shortcuts;

public class KeyboardShortcut : AShortcut
{
    public KeyboardShortcut(Key key) : base(InputSource.Keyboard, (int)key)
    {
    }

    public override float GetValue(Input input) => IsDown(input) ? 1f : 0f;

    public override bool IsUp(Input input) => input.IsKeyUp(Code.Key);

    public override bool IsPressed(Input input) => input.IsKeyPressed(Code.Key);

    public override bool IsDown(Input input) => input.IsKeyDown(Code.Key);

    public override bool IsReleased(Input input) => input.IsKeyReleased(Code.Key);
}
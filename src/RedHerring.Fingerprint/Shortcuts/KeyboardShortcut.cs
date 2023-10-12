namespace RedHerring.Fingerprint.Shortcuts;

public class KeyboardShortcut : AShortcut
{
    public KeyboardShortcut(Key key) : base(InputSource.Keyboard, (int)key)
    {
    }

    public override float GetValue(Input input) => IsDown(input) ? 1f : 0f;

    public override bool IsUp(Input input) => input.IsKeyUp(Value.Key);

    public override bool IsPressed(Input input) => input.IsKeyPressed(Value.Key);

    public override bool IsDown(Input input) => input.IsKeyDown(Value.Key);

    public override bool IsReleased(Input input) => input.IsKeyReleased(Value.Key);
}
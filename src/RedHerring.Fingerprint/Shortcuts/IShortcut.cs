namespace RedHerring.Fingerprint.Shortcuts;

public interface IShortcut
{
    IEnumerable<ShortcutValue> InputValues();
    float GetValue(Input input);
    bool IsUp(Input input);
    bool IsPressed(Input input);
    bool IsDown(Input input);
    bool IsReleased(Input input);
}
namespace RedHerring.Fingerprint.Shortcuts;

public interface IShortcut
{
    float GetValue(Input input);
    bool IsUp(Input input);
    bool IsPressed(Input input);
    bool IsDown(Input input);
    bool IsReleased(Input input);
}
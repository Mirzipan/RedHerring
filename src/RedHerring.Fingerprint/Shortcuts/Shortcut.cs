namespace RedHerring.Fingerprint.Shortcuts;

public interface Shortcut
{
    void InputCodes(IList<InputCode> result);
    float Value(Input input);
    bool IsUp(Input input);
    bool IsPressed(Input input);
    bool IsDown(Input input);
    bool IsReleased(Input input);
}
namespace RedHerring.Fingerprint.Shortcuts;

public interface Shortcut
{
    void InputCodes(IList<InputCode> result);
    float Value(InteractionContext interactionContext);
    bool IsPressed(InteractionContext interactionContext);
    bool IsDown(InteractionContext interactionContext);
    bool IsReleased(InteractionContext interactionContext);
}
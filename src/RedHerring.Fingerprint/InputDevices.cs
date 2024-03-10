namespace RedHerring.Fingerprint;

internal interface InputDevices
{
    void NextFrame();
    event Action<InputEvent>? InputEvent;
    event Action<int, char>? CharacterTyped;
}
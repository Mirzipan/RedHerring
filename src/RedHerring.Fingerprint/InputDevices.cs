namespace RedHerring.Fingerprint;

internal interface InputDevices
{
    void NextFrame();
    event Action<InputChanged>? InputChanged;
    event Action<int, char>? CharacterTyped;
}
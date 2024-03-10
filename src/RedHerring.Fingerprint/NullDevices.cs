namespace RedHerring.Fingerprint;

internal sealed class NullDevices : InputDevices
{
    public event Action<InputEvent>? InputEvent;
    public event Action<int, char>? CharacterTyped;
    
    public NullDevices()
    {
        
    }

    public void NextFrame()
    {
    }
}
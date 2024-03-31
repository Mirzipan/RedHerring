namespace RedHerring.Inputs;

internal sealed class NullDevices : InputDevices
{
    public event Action<InputChanged>? InputChanged;
    public event Action<int, char>? CharacterTyped;
    
    public NullDevices()
    {
        
    }

    public void NextFrame()
    {
    }
}
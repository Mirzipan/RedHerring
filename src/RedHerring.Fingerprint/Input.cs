using Silk.NET.Input;

namespace RedHerring.Fingerprint;

public class Input: IDisposable
{
    private List<Key> _pressed = new List<Key>();

    public void Dispose()
    {
        // TODO release managed resources here
    }
}
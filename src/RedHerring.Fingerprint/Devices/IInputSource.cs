using Silk.NET.Input;

namespace RedHerring.Fingerprint.Devices;

public interface IInputSource
{
    public string Name { get; }
    IInputDevice Device { get; }
    void Update();
}
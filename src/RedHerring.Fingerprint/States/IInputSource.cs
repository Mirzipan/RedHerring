using Silk.NET.Input;

namespace RedHerring.Fingerprint.States;

public interface IInputSource
{
    public string Name { get; }
    IInputDevice Device { get; }
    void Reset();
}
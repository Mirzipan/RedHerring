namespace RedHerring.Fingerprint.Devices;

public interface IInputState
{
    string Name { get; }
    int Priority { get; set; }
}
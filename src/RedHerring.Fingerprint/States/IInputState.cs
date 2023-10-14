namespace RedHerring.Fingerprint.States;

public interface IInputState
{
    string Name { get; }
    int Priority { get; set; }
}
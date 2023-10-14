namespace RedHerring.Fingerprint.States;

public interface IActionState : IInputState
{
    bool IsActionUp(string action);
    bool IsActionPressed(string action);
    bool IsActionDown(string action);
    bool IsActionReleased(string action);
    bool IsAnyActionDown();
    void GetActionsDown(IList<string> actions);
}
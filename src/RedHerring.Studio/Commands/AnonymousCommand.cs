namespace RedHerring.Studio.Commands;

public sealed class AnonymousCommand : Command
{
    private readonly Action _do;
    private readonly Action _undo;
    
    public AnonymousCommand(Action @do, Action undo)
    {
        _do = @do;
        _undo = undo;
    }

    public override void Do() => _do.Invoke();
    public override void Undo() => _undo.Invoke();
}
namespace RedHerring.Studio.Commands;

public sealed class CompositeCommand : Command
{
    private readonly Command[] _subCommands;
    
    public CompositeCommand(params Command[] commands)
    {
        _subCommands = commands;
    }

    public CompositeCommand(IEnumerable<Command> commands)
    {
        _subCommands = commands.ToArray();
    }
    
    public override void Do()
    {
        for (int i = 0; i < _subCommands.Length; i++)
        {
            _subCommands[i].Do();
        }
    }

    public override void Undo()
    {
        for (int i = _subCommands.Length - 1; i >= 0; i--)
        {
            _subCommands[i].Undo();
        }
    }
}
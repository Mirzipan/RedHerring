namespace RedHerring.Studio.Commands;

public sealed class CommandHistory
{
    public int Capacity { get; set; } = byte.MaxValue;

    private List<Command> _stack = new();
    private int _executionIndex;
    
    private LinkedList<Command?> _commands = new();
    private LinkedListNode<Command?>? _current;
    private LinkedListNode<Command?> _root;

    public CommandHistory()
    {
        _executionIndex = -1;
        
        _root = new LinkedListNode<Command?>(null);
        _commands.AddFirst(_root);
        _current = _root;
    }

    public void Commit(Command command)
    {
        command.Do();

        RemoveAfter(_executionIndex);
        
        _executionIndex = _stack.Count;
        _stack.Add(command);
        
        if (_stack.Count > Capacity)
        {
            _stack.RemoveAt(0);
            --_executionIndex;
        }
    }
    
    public void Undo()
    {
        if (_executionIndex >= 0)
        {
            var command = _stack[_executionIndex];
            command.Undo();

            --_executionIndex;
        }
    }

    public void Redo()
    {
        if (_executionIndex < _stack.Count - 1)
        {
            ++_executionIndex;

            var command = _stack[_executionIndex];
            command.Do();
        }
    }

    private void RemoveAfter(int index)
    {
        for (int i = _stack.Count - 1; i > index; i--)
        {
            _stack.RemoveAt(i);
        }
    }
}
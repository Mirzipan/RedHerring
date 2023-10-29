namespace RedHerring.Studio.Commands;

public sealed class CommandHistory
{
    public int Capacity { get; set; } = byte.MaxValue;

    private List<ACommand> _stack = new();
    private int _executionIndex;
    
    private LinkedList<ACommand?> _commands = new();
    private LinkedListNode<ACommand?>? _current;
    private LinkedListNode<ACommand?> _root;

    public CommandHistory()
    {
        _executionIndex = -1;
        
        _root = new LinkedListNode<ACommand?>(null);
        _commands.AddFirst(_root);
        _current = _root;
    }

    public void Commit(ACommand command)
    {
        command.Do();

        RemoveAfter(_executionIndex);
        
        _executionIndex = _stack.Count;
        _stack.Add(command);
        
        if (_stack.Count > Capacity)
        {
            _stack.RemoveAt(0);
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
        var command = _stack[_executionIndex];
        command.Do();
            
        ++_executionIndex;
    }

    private void RemoveAfter(int index)
    {
        for (int i = _stack.Count - 1; i > index; i--)
        {
            _stack.RemoveAt(i);
        }
    }
}
namespace RedHerring.Studio.Commands;

public class CommandHistoryWithChange : CommandHistory
{
	public bool WasChange { get; private set; } = false;

	public CommandHistoryWithChange(int capacity = byte.MaxValue) : base(capacity)
	{
	}

	public void ResetChange()
	{
		WasChange = false;
	}

	public override void Commit(Command command)
	{
		base.Commit(command);
		WasChange = true;
	}
    
	public override void Undo()
	{
		base.Undo();
		WasChange = true;
	}

	public override void Redo()
	{
		base.Redo();
		WasChange = true;
	}
}
namespace RedHerring.Studio.Models.Project;

public sealed class ProjectTask
{
	private Action<CancellationToken> _action;

	public ProjectTask(Action<CancellationToken> action)
	{
		_action = action;
	}

	public void Process(CancellationToken cancellationToken)
	{
		_action.Invoke(cancellationToken);
	}
}
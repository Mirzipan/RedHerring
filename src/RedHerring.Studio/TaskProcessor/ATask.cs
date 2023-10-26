namespace RedHerring.Studio.TaskProcessor;

public abstract class ATask
{
	public abstract void Process(CancellationToken cancellationToken);
}
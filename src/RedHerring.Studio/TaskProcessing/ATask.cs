namespace RedHerring.Studio.TaskProcessing;

public abstract class ATask
{
	public abstract void Process(CancellationToken cancellationToken);
}
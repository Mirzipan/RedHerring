namespace RedHerring.Studio.Models.TaskProcessor;

public abstract class AbstractTask
{
	public abstract void Process(CancellationToken cancellationToken);
}
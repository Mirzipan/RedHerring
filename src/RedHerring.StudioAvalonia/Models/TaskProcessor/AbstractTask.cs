using System.Threading;

namespace RedHerring.Studio.Models;

public abstract class AbstractTask
{
	public abstract void Process(CancellationToken cancellationToken);
}
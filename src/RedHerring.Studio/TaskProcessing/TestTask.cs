namespace RedHerring.Studio.TaskProcessing;

public class TestTask : ATask
{
	private int _id;
	
	public TestTask(int id)
	{
		_id = id;
	}
	
	public override void Process(CancellationToken cancellationToken)
	{
		Console.WriteLine($"Task {_id} started");
		Random rnd = new();
		Thread.Sleep(rnd.Next(1000, 5000));
		Console.WriteLine($"Task {_id} finished");
	}
}
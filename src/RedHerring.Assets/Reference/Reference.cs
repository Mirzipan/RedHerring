namespace RedHerring.Assets;

public abstract class Reference
{
	public readonly string Path;

	public Reference(string path)
	{
		Path = path;
	}
}

public abstract class Reference<T> : Reference where T : class
{
	private T? _value = null;
	public  T? Value => _value ??= LoadValue();
	
	protected Reference(string path) : base(path)
	{
	}

	public abstract T? LoadValue();
}
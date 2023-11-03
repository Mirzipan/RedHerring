using RedHerring.Studio.Commands;

namespace RedHerring.Studio.UserInterface;

public sealed class Inspector
{
	private readonly List<object>             _sources = new();
	private          InspectorClassControl? _contentControl;

	private static int _uniqueIdGenerator = 0;
	private        int _uniqueId          = _uniqueIdGenerator++;

	private CommandHistory _commandHistory;

	public Inspector(CommandHistory commandHistory)
	{
		_commandHistory = commandHistory;
	}

	public void Init(object source)
	{
		_sources.Clear();
		_sources.Add(source);
		Rebuild();
	}

	public void Init(IReadOnlyCollection<object> sources)
	{
		_sources.Clear();
		_sources.AddRange(sources);
		Rebuild();
	}

	public void Update()
	{
		_contentControl?.Update();
	}

	public void Commit(ACommand command)
	{
		_commandHistory.Commit(command);
	}

	#region Private
	private void Rebuild()
	{
		_contentControl = null;
		if (_sources.Count == 0)
		{
			return;
		}

		string id = $"##{_uniqueId.ToString()}";
		
		_contentControl = new InspectorClassControl(this, id);
		_contentControl.InitFromSource(null, _sources[0]);
		_contentControl.SetCustomLabel(null);
		for(int i=1;i <_sources.Count;i++)
		{
			_contentControl.AdaptToSource(null, _sources[i]);
		}
	}
	#endregion
}

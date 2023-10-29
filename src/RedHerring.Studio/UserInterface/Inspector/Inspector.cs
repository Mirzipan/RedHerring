using RedHerring.Studio.Commands;
using RedHerring.Studio.UserInterface.Attributes;

namespace RedHerring.Studio.UserInterface;

public sealed class Inspector
{
	private readonly List<object>             _sources = new();
	private          InspectorFoldoutControl? _contentControl;

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

	public void Commit(Action @do, Action undo)
	{
		_commandHistory.Commit(new AnonymousCommand(@do, undo));
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
		
		_contentControl = new InspectorFoldoutControl(this, id);
		_contentControl.InitFromSource(_sources[0]);
		_contentControl.SetCustomLabel(_sources.Count == 1 ? "Editing 1 object" : $"Editing {_sources.Count} objects");
		for(int i=1;i <_sources.Count;i++)
		{
			_contentControl.AdaptToSource(_sources[i]);
		}
	}
	#endregion
}

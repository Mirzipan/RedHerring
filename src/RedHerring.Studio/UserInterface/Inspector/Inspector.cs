using RedHerring.Studio.UserInterface.Attributes;

namespace RedHerring.Studio.UserInterface;

public sealed class Inspector
{
	private readonly List<object>             _sources = new();
	private          InspectorFoldoutControl? _contentControl;

	private static int _uniqueIdGenerator = 0;
	private        int _uniqueId          = _uniqueIdGenerator++;
	
	private InspectorTest _test  = new();                                            // TODO debug
	private List<object>  _tests = new(){new InspectorTest2(), new InspectorTest()}; // TODO debug

	public Inspector()
	{
		Init(_tests); // TODO debug
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

	#region Private
	private void Rebuild()
	{
		_contentControl = null;
		if (_sources.Count == 0)
		{
			return;
		}

		string id = $"##{_uniqueId.ToString()}";
		
		_contentControl = new InspectorFoldoutControl(id);
		_contentControl.InitFromSource(_sources[0]);
		_contentControl.SetCustomLabel(_sources.Count == 1 ? "Editing 1 object" : $"Editing {_sources.Count} objects");
		for(int i=1;i <_sources.Count;i++)
		{
			_contentControl.AdaptToSource(_sources[i]);
		}
	}
	#endregion
}


public class InspectorTest
{
	[ReadOnlyInInspector] public int SomeValue1 = 1;
	public int SomeValue2 = 22;
	public int SomeValue3 = 333;
	public int SomeValue4 = 4444;
	
	public InspectorTestSubclass Subclass = new();
}

public class InspectorTest2
{
	public int SomeValue1 = 5;
	[ReadOnlyInInspector] public int SomeValue2 = 22;

	public InspectorTestSubclass2 Subclass = new();

	[ShowInInspector] private int SomeValue3 = 333;
	[HideInInspector] public  int SomeValue4 = 4444;
	
	public int SomeValue5 = 55555;
}

public class InspectorTestSubclass
{
	public int SubValue1 = 111111;
	public int SubValue2 = 222222;
}

public class InspectorTestSubclass2
{
	public int SubValue2 = 666666;
	public int SubValue3 = 555555;
}
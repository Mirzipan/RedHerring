using IconFonts;
using RedHerring.Studio.Models;
using RedHerring.Studio.UserInterface;
using RedHerring.Studio.UserInterface.Attributes;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.Tools;

[Tool(ToolName)]
public sealed class ToolInspector : Tool
{
	public const       string    ToolName = FontAwesome6.CircleInfo + " Inspector";
	protected override string    Name => ToolName;
	private readonly   Inspector _inspector;
	private            bool      _subscribedToChange = false;

	//private List<object> _tests = new(){new InspectorTest(), new InspectorTest2()}; // TODO debug
	private List<object> _tests = new(){new InspectorTest()}; // TODO debug

	public ToolInspector(StudioModel studioModel) : base(studioModel)
	{
		_inspector = new Inspector(studioModel.CommandHistory);
	}

	public ToolInspector(StudioModel studioModel, int uniqueId) : base(studioModel, uniqueId)
	{
		_inspector = new Inspector(studioModel.CommandHistory);
	}
	
	public override void Update(out bool finished)
	{
		finished = UpdateUI();
	}

	private bool UpdateUI()
	{
		bool isOpen = true;
		if (Gui.Begin(NameWithSalt, ref isOpen))
		{
			SubscribeToChange();
			_inspector.Update();
			Gui.End();
		}
		else
		{
			UnsubscribeFromChange();
		}

		return !isOpen;
	}

	private void SubscribeToChange()
	{
		if (_subscribedToChange)
		{
			return;
		}

		StudioModel.Selection.SelectionChanged += OnSelectionChanged;
		_subscribedToChange                    =  true;
		OnSelectionChanged();
	}

	private void UnsubscribeFromChange()
	{
		if (!_subscribedToChange)
		{
			return;
		}
		
		StudioModel.Selection.SelectionChanged -= OnSelectionChanged;
		_subscribedToChange                    =  false;
	}

	private void OnSelectionChanged()
	{
		_inspector.Init(StudioModel.Selection.GetAllSelectedTargets());
	}

	public void Test()
	{
		_inspector.Init(_tests);
	}
}



//=======================================================================================================
// tests
//=======================================================================================================
public enum TestEnum
{
	Abc,
	Def,
	Ghi,
	Jkl,
	Mno
}

public class InspectorTest
{
	[ReadOnlyInInspector] public int      SomeValue1 = 1;
	public                       int      SomeValue2 = 22;
	public                       int      SomeValue3 = 333;
	public                       int      SomeValue4 = 4444;
	public                       float    FloatValue = 1.0f;
	public                       bool     BoolValue  = true;
	public                       TestEnum EnumValue  = TestEnum.Def;
	
	public InspectorTestSubclass  Subclass  = new();
	public InspectorTestSubclass? Subclass2 = null;
	
	[ValueDropdown("DropdownSource")] public int      DropdownInt    = 1;
	[ValueDropdown("DropdownSource")] public string   DropdownString = "pear";
	[HideInInspector]                 public string[] DropdownSource = {"apple", "pear", "orange", "banana"};
	
	public int[]     IntArray = {1, 2, 3, 4, 5};
	public List<int> IntList  = new() {1, 2, 3, 4};
	
	public List<InspectorTestSubclass> SubClassList = new()
	                                                  {
		                                                  new InspectorTestSubclass(),
		                                                  new InspectorTestSubclass(),
	                                                  };
	
	[Button]
	private void TestMethod()
	{
		Console.WriteLine("TestMethod");
	}
}

public class InspectorTestSubclass
{
	public int    SubValue1   = 111111;
	public int    SubValue2   = 222222;
	public string StringValue = "abc";
}



public class InspectorTest2
{
	public                       int SomeValue1 = 5;
	[ReadOnlyInInspector] public int SomeValue2 = 22;

	public InspectorTestSubclass2 Subclass = new();

	[ShowInInspector] private int SomeValue3 = 333;
	[HideInInspector] public  int SomeValue4 = 4444;
	
	public int   SomeValue5 = 55555;
	public float FloatValue = 1.0f;
	public bool  BoolValue  = true;

	public TestEnum EnumValue = TestEnum.Abc;

	[ValueDropdown("DropdownSource")] public int      DropdownInt    = 1;
	[ValueDropdown("DropdownSource")] public string   DropdownString = "pear";
	[HideInInspector]                 public string[] DropdownSource = {"apple", "pear", "orange", "banana"};

	public int[]     IntArray = {1, 2, 3, 4, 5};
	public List<int> IntList  = new() {1, 2, 3, 4, 5};
	
	[Button]
	private void TestMethod()
	{
		Console.WriteLine("TestMethod2");
	}
}

public class InspectorTestSubclass2
{
	public int    SubValue2   = 666666;
	public int    SubValue3   = 555555;
	public string StringValue = "abc";
}
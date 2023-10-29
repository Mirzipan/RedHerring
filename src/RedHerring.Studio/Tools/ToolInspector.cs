using RedHerring.Studio.Commands;
using RedHerring.Studio.Models;
using RedHerring.Studio.Systems;
using RedHerring.Studio.UserInterface;
using RedHerring.Studio.UserInterface.Attributes;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.Tools;

public sealed class ToolInspector : ATool
{
	protected override string    Name => "Inspector";
	private readonly   Inspector _inspector;

	private List<object>  _tests = new(){new InspectorTest2(), new InspectorTest()}; // TODO debug
	
	public ToolInspector(StudioModel studioModel, CommandHistory commandHistory) : base(studioModel)
	{
		_inspector = new Inspector(commandHistory);
		_inspector.Init(_tests);
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
			_inspector.Update();
			Gui.End();
		}

		return !isOpen;
	}
}



//=======================================================================================================
// tests
//=======================================================================================================
public class InspectorTest
{
	[ReadOnlyInInspector] public int SomeValue1 = 1;
	public                       int SomeValue2 = 22;
	public                       int SomeValue3 = 333;
	public                       int SomeValue4 = 4444;
	
	public InspectorTestSubclass Subclass = new();
}

public class InspectorTestSubclass
{
	public int SubValue1 = 111111;
	public int SubValue2 = 222222;
}



public class InspectorTest2
{
	public                       int SomeValue1 = 5;
	[ReadOnlyInInspector] public int SomeValue2 = 22;

	public InspectorTestSubclass2 Subclass = new();

	[ShowInInspector] private int SomeValue3 = 333;
	[HideInInspector] public  int SomeValue4 = 4444;
	
	public int SomeValue5 = 55555;
}

public class InspectorTestSubclass2
{
	public int SubValue2 = 666666;
	public int SubValue3 = 555555;
}
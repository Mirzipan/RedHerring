using System.Reflection;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface;

public sealed class InspectorButtonControl : AnInspectorControl
{
	private object     _source;
	private MethodInfo _method;
	
	public InspectorButtonControl(Inspector inspector, string id, string label, object source, MethodInfo method) : base(inspector, id)
	{
		Label   = label;
		LabelId = $"{Label}{Id}";
		_source = source;
		_method = method;
	}

	public override void Update()
	{
		if (Gui.Button(LabelId))
		{
			_method.Invoke(_source, null);
		}
	}
}
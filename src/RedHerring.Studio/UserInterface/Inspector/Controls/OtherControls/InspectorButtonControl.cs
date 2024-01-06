using System.Reflection;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface;

public sealed class InspectorButtonControl : InspectorControl
{
	private readonly struct Binding
	{
		public readonly object Source;

		public Binding(object source, MethodInfo method)
		{
			Source = source;
			Method = method;
		}

		public readonly MethodInfo Method;
	}
	
	private List<Binding> _bindings = new();

	public InspectorButtonControl(IInspectorCommandTarget commandTarget, string id, string label, object source, MethodInfo method) : base(commandTarget, id)
	{
		Label   = label;
		LabelId = $"{Label}##{Id}";
		_bindings.Add(new Binding(source, method));
	}

	public void AddBinding(object sourceFieldValue, MethodInfo method)
	{
		_bindings.Add(new Binding(sourceFieldValue, method));
	}
	
	public override void Update()
	{
		if (Gui.Button(LabelId))
		{
			_bindings.ForEach(binding => binding.Method.Invoke(binding.Source, null));
		}
	}
}
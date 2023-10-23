namespace RedHerring.Studio.ViewModels.Inspector;

public sealed class InspectorBoolViewModel : InspectorItemViewModel
{
	public bool Value { get; set; }
	
	public InspectorBoolViewModel(string name, bool value) : base(name)
	{
		Value = value;
	}
}
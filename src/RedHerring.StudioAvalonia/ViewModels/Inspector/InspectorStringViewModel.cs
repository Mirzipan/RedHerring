namespace RedHerring.Studio.ViewModels.Inspector;

public sealed class InspectorStringViewModel : InspectorItemViewModel
{
	public string Value { get; set; }
	
	public InspectorStringViewModel(string name, string value) : base(name)
	{
		Value = value;
	}
}
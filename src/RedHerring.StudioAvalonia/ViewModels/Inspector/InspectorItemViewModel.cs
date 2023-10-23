namespace RedHerring.Studio.ViewModels.Inspector;

public abstract class InspectorItemViewModel
{
	public string Name { get; set; }
	
	public InspectorItemViewModel(string name)
	{
		Name = name;
	}
}
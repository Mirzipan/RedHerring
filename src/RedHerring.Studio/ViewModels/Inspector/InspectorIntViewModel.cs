namespace RedHerring.Studio.ViewModels.Inspector;

public sealed class InspectorIntViewModel : InspectorItemViewModel
{
	public int Value { get; set; }

	public InspectorIntViewModel(string name, int value) : base(name)
	{
		Value = value;
	}
}
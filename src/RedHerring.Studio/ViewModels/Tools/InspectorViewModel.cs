using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using RedHerring.Studio.ViewModels.Inspector;

namespace RedHerring.Studio.ViewModels;

public partial class InspectorViewModel : ViewModelBase
{
	[ObservableProperty]
	private List<InspectorItemViewModel> _items = new();

	public InspectorViewModel()
	{
		_items.Add(new InspectorIntViewModel("Int value", 42));
		_items.Add(new InspectorBoolViewModel("Bool value", true));
		_items.Add(new InspectorStringViewModel("String value", "Hello world!"));
	}
}
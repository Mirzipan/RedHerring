using System.Collections.Generic;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using RedHerring.Studio.Models;

namespace RedHerring.Studio.ViewModels;

public partial class ProjectViewModel : ViewModelBase
{
	[ObservableProperty]
	private List<FileSystemNode> _roots = new(); 

	public ProjectViewModel()
	{
		Model.Project.PropertyChanged += OnProjectPropertyChanged;
		
		// FolderNode root = new("root");
		// _roots.Add(root);
		//
		// root.Children.Add(new FileNode("file 1"));
		//
		// FolderNode folder = new FolderNode("directory 1");
		// for (int i = 0; i < 100; ++i)
		// {
		// 	folder.Children.Add(new FileNode($"sub file {i}"));
		// }
		// root.Children.Add(folder);
		//
		// root.Children.Add(new FileNode("file 2"));
		// root.Children.Add(new FileNode("file 3"));
		// root.Children.Add(new FileNode("file 4 (this is very long child name that should cause horizontal scrolling hopefully)"));
	}

	private void OnProjectPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == nameof(ProjectModel.AssetsFolder))
		{
			Roots = new List<FileSystemNode>{Model.Project.AssetsFolder};
		}
	}
}

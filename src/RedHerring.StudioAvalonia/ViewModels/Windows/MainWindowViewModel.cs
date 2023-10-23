using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RedHerring.Studio.Models;

namespace RedHerring.Studio.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
	[ObservableProperty] private string _statusBarMessage = "Ready";
	[ObservableProperty] private IBrush _statusBarColor = Brushes.DarkGreen;

	public event EventHandler? OnClose;

	[ObservableProperty] private ProjectViewModel _project = new();
	[ObservableProperty] private ConsoleViewModel _console = new();
	[ObservableProperty] private InspectorViewModel _inspector = new();

	public MainWindowViewModel()
	{
		Model.Instance.TaskProcessor.OnChanged += OnTaskProcessorChanged;
		RefreshStatusBarMessage();
	}

	private void OnTaskProcessorChanged(object sender)
	{
		RefreshStatusBarMessage();
	}

	private void RefreshStatusBarMessage()
	{
		int workerThreadsCount = Model.Instance.TaskProcessor.WorkerThreadsCount;
		int remainingTasks     = Model.Instance.TaskProcessor.GetRemainingTasks();
		int availableThreads   = Model.Instance.TaskProcessor.AvailableWorkerThreads;
		
		if (remainingTasks > 0)
		{
			StatusBarMessage = $"Processing {remainingTasks} tasks on {workerThreadsCount} threads.";
		}
		else
		{
			StatusBarMessage = $"Ready. {availableThreads} of {workerThreadsCount} threads available.";
		}
		
		StatusBarColor = remainingTasks == 0 && availableThreads == workerThreadsCount ? Brushes.DarkGreen : Brushes.SaddleBrown;
	}

	[RelayCommand]
	public void OnFileNewProject()
	{
		
	}

	[RelayCommand]
	public async Task OnFileOpenProject()
	{
		FolderPickerOpenOptions options = new()
		                                  {
			                                  Title         = "Select project folder",
			                                  AllowMultiple = false,
		                                  };
		IReadOnlyList<IStorageFolder> result = await WindowService.OpenFolderPickerAsync(options);
		if (result.Count != 1)
		{
			return;
		}

		Model.Project.Open(result[0].Path.AbsolutePath);
	}
	
	[RelayCommand]
	public void OnFileExit()
	{
		OnClose?.Invoke(this, EventArgs.Empty);
	}

	[RelayCommand]
	public void OnDebugTaskProcessorTest()
	{
		Model.Instance.RunTests();
	}

	[RelayCommand]
	public void OnDebugSerializationTest()
	{
		SerializationTests.Test();
	}

	[RelayCommand]
	public void OnDebugImporterTest()
	{
		ImporterTests.Test();
	}
}
using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using RedHerring.Studio.Models;
using RedHerring.Studio.ViewModels;
using RedHerring.Studio.Views;

namespace RedHerring.Studio;

public partial class App : Application
{
	private bool _userWantsToCloseApplication = false;
	
	public override void Initialize()
	{
		AvaloniaXamlLoader.Load(this);
	}

	public override void OnFrameworkInitializationCompleted()
	{
		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
		{
			MainWindowViewModel viewModel = new ();
			desktop.MainWindow = new MainWindow
			                     {
				                     DataContext = viewModel,
			                     };

			viewModel.OnClose += (sender, ev) => desktop.MainWindow.Close();

			desktop.MainWindow.Closing += OnMainWindowClosing;
		}

		this.AttachDevTools();
		
		base.OnFrameworkInitializationCompleted();
	}

	private void OnMainWindowClosing(object? sender, WindowClosingEventArgs ev)
	{
		if (_userWantsToCloseApplication)
		{
			Model.Instance.Exit();
			return;
		}

		ev.Cancel = true;
		Dispatcher.UIThread.InvokeAsync(ConfirmExit);
	}

	private async Task ConfirmExit()
	{
		MessageBoxWindowViewModel.Result result = await WindowService.MessageBoxAsync(
			new MessageBoxWindowViewModel(
				"Exit application",
				"All unsaved changes will be lost!\nDo you want to proceed?",
				MessageBoxWindowViewModel.Style.YesNo
			));
		
		if (result == MessageBoxWindowViewModel.Result.Positive)
		{
			_userWantsToCloseApplication = true;
			(Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow?.Close();
		}
	}
}
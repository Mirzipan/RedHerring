using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using RedHerring.Studio.ViewModels;
using RedHerring.Studio.Views.Dialogs;

namespace RedHerring.Studio;

public static class WindowService
{
	private static Window? MainWindow => (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
	
	public static async Task<MessageBoxWindowViewModel.Result> MessageBoxAsync(MessageBoxWindowViewModel viewModel)
	{
		Window? ownerWindow = MainWindow;
		if (ownerWindow == null)
		{
			return MessageBoxWindowViewModel.Result.Negative;
		}

		Window window = new MessageBoxWindow {DataContext = viewModel};
		viewModel.OnClose += (sender, result) => window.Close(result);
		return await window.ShowDialog<MessageBoxWindowViewModel.Result>(ownerWindow);
	}

	public static async Task<IReadOnlyList<IStorageFolder>> OpenFolderPickerAsync(FolderPickerOpenOptions options)
	{
		if (MainWindow == null)
		{
			return new List<IStorageFolder>();
		}

		return await MainWindow.StorageProvider.OpenFolderPickerAsync(options);
	}
}
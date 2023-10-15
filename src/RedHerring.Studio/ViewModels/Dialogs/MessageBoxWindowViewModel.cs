using System;
using CommunityToolkit.Mvvm.Input;

namespace RedHerring.Studio.ViewModels;

public partial class MessageBoxWindowViewModel : ViewModelBase
{
	public enum Style
	{
		Ok,
		OkCancel,
		YesNo,
	}

	public enum Result
	{
		Negative, // <-- negative is default, will be returned if nothing was selected and window was just closed
		Positive,
	}
	
	public string                      Title                     { get; set; }
	public string                      Message                   { get; set; }
	public bool                        IsPositiveChoiceAvailable { get; set; }
	public string                      PositiveChoiceLabel       { get; set; }
	public string                      NegativeChoiceLabel       { get; set; }
	public event EventHandler<Result>? OnClose;

	public MessageBoxWindowViewModel(
		string title,
		string message,
		Style  style
	)
	{
		Title   = title;
		Message = message;
		SetupChoices(style);
	}

	[RelayCommand]
	public void OnPositiveButtonClicked()
	{
		OnClose?.Invoke(this, Result.Positive);
	}

	[RelayCommand]
	public void OnNegativeButtonClicked()
	{
		OnClose?.Invoke(this, Result.Negative);
	}

	private void SetupChoices(Style style)
	{
		switch (style)
		{
			case Style.Ok:
			case Style.OkCancel:
				PositiveChoiceLabel = "Ok";
				NegativeChoiceLabel = "Cancel";
				break;
			case Style.YesNo:
				PositiveChoiceLabel = "Yes";
				NegativeChoiceLabel = "No";
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(style), style, null);
		}

		IsPositiveChoiceAvailable = style != Style.Ok;
	}
}
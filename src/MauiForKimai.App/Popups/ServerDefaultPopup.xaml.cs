namespace MauiForKimai.Popups;

public partial class ServerDefaultPopup
{
	public ServerDefaultPopup(PopupSizeConstants popupSizeConstants)
	{
		InitializeComponent();
          
          
		Size = popupSizeConstants.Medium;
	}

	void OnYesButtonClicked(object? sender, EventArgs e) => Close(true);

	void OnNoButtonClicked(object? sender, EventArgs e) => Close(false);
}
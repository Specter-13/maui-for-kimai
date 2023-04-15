namespace MauiForKimai.Popups;

public partial class FirstStartPopup
{
	public FirstStartPopup()
	{
		InitializeComponent();
          
          
		Size = new(300, 300);
	}

	void OnYesButtonClicked(object? sender, EventArgs e) => Close(true);

	
}
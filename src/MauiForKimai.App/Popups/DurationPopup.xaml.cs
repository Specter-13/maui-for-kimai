namespace MauiForKimai.Popups;

public partial class DurationPopup 
{
	private DurationPopupViewModel _vm;
	public DurationPopup(PopupSizeConstants popupSizeConstants, DurationPopupViewModel vm)
	{
		InitializeComponent();
		Size = popupSizeConstants.Small;
		_vm = vm;
		BindingContext = _vm;
		
	}

    void OnYesButtonClicked(object sender, EventArgs e)
	{ 

		var duration = new TimeSpan(_vm.Hours, _vm.Minutes,0);
		Close(duration);
	}

	void OnNoButtonClicked(object sender, EventArgs e) => Close(null);
}
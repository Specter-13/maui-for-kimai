using MauiForKimai.ViewModels;

namespace MauiForKimai.Views;

public partial class TimeSheetView
{
	public TimeSheetView(TimeSheetViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}
using MauiForKimai.ViewModels;

namespace MauiForKimai.Views;

public partial class TimeSheetView : ContentPage
{
	public TimeSheetView(TimeSheetViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}
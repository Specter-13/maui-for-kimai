using MauiForKimai.ViewModels;

namespace MauiForKimai.Views;

public partial class TimesheetFavouritesCreateView 
{
	public TimesheetFavouritesCreateView(TimesheetFavouritesCreateViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}
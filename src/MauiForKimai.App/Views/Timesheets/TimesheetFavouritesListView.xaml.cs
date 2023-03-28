using MauiForKimai.ViewModels;

namespace MauiForKimai.Views;

public partial class TimesheetFavouritesListView
{
	public TimesheetFavouritesListView(TimesheetFavouritesListViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}
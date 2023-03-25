using MauiForKimai.ViewModels;

namespace MauiForKimai.Views;

public partial class TimesheetListView
{
	public TimesheetListView(TimesheetListViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}
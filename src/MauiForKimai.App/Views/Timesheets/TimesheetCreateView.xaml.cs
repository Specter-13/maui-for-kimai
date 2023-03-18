using MauiForKimai.ViewModels.Timesheets;

namespace MauiForKimai.Views.Timesheets;

public partial class TimesheetCreateView 
{
	public TimesheetCreateView(TimesheetCreateViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}
using MauiForKimai.ViewModels.Timesheets;
using CommunityToolkit.Maui.Views;

namespace MauiForKimai.Views.Timesheets;

public partial class TimesheetCreateView 
{
	public TimesheetCreateView(TimesheetCreateViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

}
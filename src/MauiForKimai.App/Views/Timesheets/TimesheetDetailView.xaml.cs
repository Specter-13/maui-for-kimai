using MauiForKimai.ViewModels;
using CommunityToolkit.Maui.Views;

namespace MauiForKimai.Views;

public partial class TimesheetDetailView 
{
	public TimesheetDetailView(TimesheetDetailViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

}
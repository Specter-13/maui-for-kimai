using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core.Extensions;

namespace MauiForKimai.ViewModels;

public partial class MainViewModel : ViewModelBase
{
	protected readonly ITimesheetService timesheetService;

	public MainViewModel(ITimesheetService ts, ApiStateProvider asp, IRoutingService routingService) : base(asp, routingService)
	{
		timesheetService = ts;

		
	}
	
	

	public ObservableCollection<TimesheetCollectionExpanded> RecentTimesheets {get;set; } = new();



    [RelayCommand]
    async Task GetTimeSheets()
    {
		IsBusy = true;

		var timeheets = (await timesheetService.GetTenRecentTimesheetsAsync()).ToObservableCollection();

		RecentTimesheets.Clear();
		foreach(var timesheet in timeheets)
		{ 
			RecentTimesheets.Add(timesheet);
		}


		string text = "Timesheets returned!";
		ToastDuration duration = ToastDuration.Short;
		double fontSize = 14;

		var toast = Toast.Make(text, duration, fontSize);
		CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
		await toast.Show(cancellationTokenSource.Token);

		IsBusy = false;
        
    }
}


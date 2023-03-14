using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core.Extensions;

namespace MauiForKimai.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
	protected readonly ITimesheetService timesheetService;

	public HomeViewModel(ITimesheetService ts, ApiStateProvider asp, IRoutingService routingService) : base(asp, routingService)
	{
		timesheetService = ts;

	}
	
	public override async Task OnAppearing()
	{
		await GetTimeSheets();
		var activeTimesheets = await timesheetService.GetActive();
		if (activeTimesheets.Any()) 
		{
			ActiveTimesheetId = (int)activeTimesheets.First().Id;
		}
	}

	public ObservableCollection<TimesheetCollectionExpanded> RecentTimesheets {get;set; } = new();

	[ObservableProperty]
	int activeTimesheetId;

	[RelayCommand]
	async Task StartNewTimesheet()
	{
		var timesheet = new TimesheetEditForm
		{ 
			Begin = DateTimeOffset.Now,
			Project = 1,
			Activity = 1,
			Billable = true,
		};

		var active = await timesheetService.Create(timesheet);
		ActiveTimesheetId = (int)active.Id;
	}

	[RelayCommand]
	async Task StopActiveTimesheet()
	{
		if(ActiveTimesheetId != 0)
		{ 

			await timesheetService.StopActive(ActiveTimesheetId);
			ActiveTimesheetId = 0;
		}
	}

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

		IsBusy = false;
        
    }
}


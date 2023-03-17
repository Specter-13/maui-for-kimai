using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core.Extensions;
using System.Numerics;

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

	[ObservableProperty]
	bool isTimetrackingActive;


	private IDispatcherTimer _timer {get;set;}

	[RelayCommand]
	async Task StartNewTimesheet()
	{
		_timer = Application.Current.Dispatcher.CreateTimer();
		_timer.Interval = TimeSpan.FromMilliseconds(1000);
		_timer.Tick += (s, e) =>
		{
			_seconds += 1;
			Time = TimeSpan.FromSeconds(_seconds);
		};

		

		var timesheet = new TimesheetEditForm
		{ 
			Begin = DateTimeOffset.Now,
			Project = 1,
			Activity = 1,
			Billable = true,
		};

		_timer.Start();
		//var active = await timesheetService.Create(timesheet);
		//ActiveTimesheetId = (int)active.Id;
		IsTimetrackingActive = true;

	}

	[ObservableProperty]
    public TimeSpan time = new TimeSpan();

	private uint _seconds;

    [RelayCommand]
	async Task StopActiveTimesheet()
	{
		
		
		if (ActiveTimesheetId != 0)
		{

			//await timesheetService.StopActive(ActiveTimesheetId);
			
			ActiveTimesheetId = 0;
		}
		_timer.Stop();
		_seconds = 0;
		IsTimetrackingActive = false;

	}
	private void timer_Tick(object sender, EventArgs e)
	{
		_seconds += 1;
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

	[RelayCommand]
    async Task TimesheetOnTap(TimesheetCollectionExpanded currentTimesheet)
    {
		var x = 10;
		var z = currentTimesheet;
        
    }
}


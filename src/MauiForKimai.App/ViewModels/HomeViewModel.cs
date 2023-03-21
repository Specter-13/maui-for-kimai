using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core.Extensions;
using System.Numerics;
using MauiForKimai.ViewModels.Timesheets;
using MauiForKimai.Models;

namespace MauiForKimai.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
	protected readonly ITimesheetService timesheetService;

	public HomeViewModel(ITimesheetService ts, ApiStateProvider asp, IRoutingService routingService) : base(asp, routingService)
	{
		timesheetService = ts;
		CreateTimer();

	}

	private void CreateTimer()
	{ 
		_timer = Application.Current.Dispatcher.CreateTimer();
		_timer.Interval = TimeSpan.FromMilliseconds(1000);
		_timer.Tick += (s, e) =>
		{
			_seconds += 1;
			Time = TimeSpan.FromSeconds(_seconds);
		};
	}
	
	public override async Task OnAppearing()
	{
		if(base.ApiStateProvider.IsAuthenticated)
		{ 
			GetTimeSheets();
			var activeTimesheets = await timesheetService.GetActive();
			if (activeTimesheets.Any()) 
			{
				ActiveTimesheetId = (int)activeTimesheets.First().Id;
			}
		}
	}

	public ObservableCollection<TimesheetRecentListModel> RecentTimesheets {get;set; } = new();

	[ObservableProperty]
	int activeTimesheetId;

	[ObservableProperty]
	bool isTimetrackingActive;


	private IDispatcherTimer _timer {get;set;}

	[ObservableProperty]
	private TimesheetEditForm actualTimesheet;

	[ObservableProperty]
    public TimeSpan time = new TimeSpan();

	private uint _seconds;

	[RelayCommand]
	async Task StartNewTimesheet()
	{
		
		ActualTimesheet = new TimesheetEditForm
		{ 
			Begin = DateTimeOffset.Now
		};
		_timer.Start();
		IsTimetrackingActive = true;
		//var active = await timesheetService.Create(timesheet);
		//ActiveTimesheetId = (int)active.Id;
		

	}

	[RelayCommand]
	async Task GoToLogin()
	{	
		var route = base.RoutingService.GetRouteByViewModel<LoginViewModel>();
		await Navigation.NavigateTo(route);
	}

    [RelayCommand]
	async Task StopActiveTimesheet()
	{	
		if (ActiveTimesheetId != 0)
		{
			//await timesheetService.StopActive(ActiveTimesheetId);
			ActiveTimesheetId = 0;
		}
		_timer.Stop();
		ActualTimesheet.End = DateTimeOffset.Now;
		_seconds = 0;
		Time = new TimeSpan();
		IsTimetrackingActive = false;

		var route = RoutingService.GetRouteByViewModel<TimesheetCreateViewModel>();
		await Navigation.NavigateTo(route, ActualTimesheet);

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
			RecentTimesheets.Add((TimesheetRecentListModel)timesheet);
		}

		IsBusy = false;
        
    }

	[RelayCommand]
    async Task TimesheetOnTap(TimesheetRecentListModel currentTimesheet)
    {
		var x = 10;
		var z = currentTimesheet;
        
    }
}


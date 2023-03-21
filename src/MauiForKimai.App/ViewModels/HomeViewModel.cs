using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core.Extensions;
using System.Numerics;
using MauiForKimai.ViewModels.Timesheets;
using MauiForKimai.Models;
using CommunityToolkit.Mvvm.Messaging;
using MauiForKimai.Messenger;
using MauiForKimai.ApiClient.Services;

namespace MauiForKimai.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
	protected readonly ITimesheetService timesheetService;
	protected readonly ILoginService _loginService;
	public HomeViewModel(ITimesheetService ts, ApiStateProvider asp, IRoutingService routingService, ILoginService loginService) : base(asp, routingService)
	{
		timesheetService = ts;
		_loginService = loginService;
		CreateTimer();

		 WeakReferenceMessenger.Default.Register<TimesheetStartMessage>(this, (r, m) =>
        {
            ActiveTimesheet = m.Value;
			_timer.Start();

			timesheetService.Create(ActiveTimesheet);
        });

	}

	[ObservableProperty]
	TimesheetEditForm actualProject;
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
		else
		{
			NetworkAccess accessType = Connectivity.Current.NetworkAccess;

			if (accessType == NetworkAccess.Internet)
			{
				// Connection to internet is available
				var status = await _loginService.LoginToDefaultOnStartUp();
				if(status == true)
				{ 

					double fontSize = 14;

					var toast = Toast.Make("Connection to Kimai established!", ToastDuration.Short, 14);
					await toast.Show();

					base.ApiStateProvider.SetIsAuthenticated();

					GetTimeSheets();
					var activeTimesheets = await timesheetService.GetActive();
					if (activeTimesheets.Any()) 
					{
						ActiveTimesheetId = (int)activeTimesheets.First().Id;
					}
				}
				else
				{ 
					var toast = Toast.Make("Connection Failed!", ToastDuration.Short, 14);
					await toast.Show();
					base.ApiStateProvider.SetIsAuthenticated();
				}
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
	private TimesheetEditForm activeTimesheet;

	[ObservableProperty]
    public TimeSpan time = new TimeSpan();

	private uint _seconds;

	[RelayCommand]
	async Task StartNewTimesheet()
	{
		
		//ActiveTimesheet = new TimesheetEditForm
		//{ 
		//	Begin = DateTimeOffset.Now
		//};

		var route = RoutingService.GetRouteByViewModel<TimesheetCreateViewModel>();
		await Navigation.NavigateTo(route);


		//_timer.Start();
		//IsTimetrackingActive = true;
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
		//timesheetService.StopActive(ActiveTimesheet.);
		_timer.Stop();
		ActiveTimesheet.End = DateTimeOffset.Now;
		_seconds = 0;
		Time = new TimeSpan();
		IsTimetrackingActive = false;

		//var route = RoutingService.GetRouteByViewModel<TimesheetCreateViewModel>();
		//await Navigation.NavigateTo(route, ActualTimesheet);

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


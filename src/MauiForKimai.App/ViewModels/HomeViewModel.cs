﻿using CommunityToolkit.Maui.Alerts;
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
	public HomeViewModel(IRoutingService rs, ILoginService ls, ITimesheetService ts, ApiStateProvider asp) : base(rs, ls)
	{
		timesheetService = ts;
		_loginService = ls;
		CreateTimer();
		RegisterMessages();
		

	}

	private void RegisterMessages()
	{ 
		 WeakReferenceMessenger.Default.Register<TimesheetStartMessage>(this, async (r, m) =>
        {
            var timesheetEditForm = m.Value;
			ActiveTimesheet = await timesheetService.Create(timesheetEditForm);
			_timer.Start();
			IsTimetrackingActive = true;
        });
	}


	[ObservableProperty]
	private TimesheetEntity activeTimesheet;

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
	private uint _seconds;
	private void timer_Tick(object sender, EventArgs e)
	{
		_seconds += 1;
	}
	
	public override async Task OnAppearing()
	{
		if(base.ApiStateProvider.IsAuthenticated)
		{ 
			GetTimeSheets();
			var activeTimesheets = await timesheetService.GetActive();
			if (activeTimesheets.Any()) 
			{
				//ActiveTimesheetId = (int)activeTimesheets.First().Id;
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

					var toast = Toast.Make("Connection to Kimai established!", ToastDuration.Short, 14);
					await toast.Show();

					base.ApiStateProvider.SetIsAuthenticated();

					GetTimeSheets();
					var activeTimesheets = await timesheetService.GetActive();
					if (activeTimesheets.Any()) 
					{
						//ActiveTimesheetId = (int)activeTimesheets.First().Id;
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
	bool isTimetrackingActive;


	private IDispatcherTimer _timer {get;set;}



	[ObservableProperty]
    public TimeSpan time = new TimeSpan();



	[RelayCommand]
	async Task StartTimeTracking()
	{
		var route = routingService.GetRouteByViewModel<TimesheetCreateViewModel>();
		await Navigation.NavigateTo(route,"cau");

	}

	[RelayCommand]
	async Task StopTimeTracking()
	{	
		var stopped = await timesheetService.StopActive(ActiveTimesheet.Id.Value);
		_timer.Stop();
		IsTimetrackingActive = false;
		_seconds = 0;
		Time = new TimeSpan();


		

	}


	[RelayCommand]
	async Task GoToLogin()
	{	
		var route = base.routingService.GetRouteByViewModel<ServerListViewModel>();
		await Navigation.NavigateTo(route);
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


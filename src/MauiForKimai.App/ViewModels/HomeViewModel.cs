using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core.Extensions;
using System.Numerics;
using MauiForKimai.ViewModels.Timesheets;
using CommunityToolkit.Mvvm.Messaging;
using MauiForKimai.Messenger;
using MauiForKimai.ApiClient.Services;
using MauiForKimai.Wrappers;

namespace MauiForKimai.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
	protected readonly ITimesheetService timesheetService;
	protected readonly ILoginService _loginService;
	private readonly IDispatcherWrapper _dispatcherWrapper;
	public HomeViewModel(IRoutingService rs, ILoginService ls, ITimesheetService ts, ApiStateProvider asp, IDispatcherWrapper dispatcherWrapper) : base(rs, ls)
	{
		timesheetService = ts;
		_loginService = ls;
		_dispatcherWrapper = dispatcherWrapper ?? new DispatcherWrapper(Application.Current.Dispatcher);
		CreateTimer();
		RegisterMessages();
	}
	// Initialize methods
	private void RegisterMessages()
	{ 
		 WeakReferenceMessenger.Default.Register<TimesheetStartMessage>(this, async (r, m) =>
        {
            var timesheetEditForm = m.Value;
			//await timesheetService.CreateExpanded(timesheetEditForm);
			await timesheetService.Create(timesheetEditForm);
			_timer.Start();
			IsTimetrackingActive = true;
			var activeTimesheet = (await timesheetService.GetActive()).FirstOrDefault();
			ActiveTimesheet =  activeTimesheet.ToTimesheetActiveModel();
        });
	}

	public override async Task Initialize()
    {
		var isSuccessfull = await _loginService.LoginToDefaultOnStartUp();
		IToast toast;

		if(isSuccessfull)
		{
			toast = Toast.Make("Connection to Kimai established!", ToastDuration.Short, 14);
		}
		else
		{
			toast = Toast.Make("Connection to Kimai failed!", ToastDuration.Short, 14);
		}
		await toast.Show();
    }

	public override async Task OnAppearing()
	{

		if (base.ApiStateProvider.IsAuthenticated && base.GetConnectivity == NetworkAccess.Internet)
		{
			IsBusy = true;
			// Connection to internet is available
			await GetTimeSheets();
			await TryToGetActiveTimesheet();
			IsBusy = false;
		}
		else
		{ 
			var toast = Toast.Make("Cannot load data!", ToastDuration.Short, 14);
			await toast.Show();
		}

	}

	//Properties
	[ObservableProperty]
	private TimesheetActiveModel activeTimesheet;
	public ObservableCollection<TimesheetRecentListModel> RecentTimesheets {get;set; } = new();

	[ObservableProperty]
	bool isTimetrackingActive;

	

	[ObservableProperty]
    public TimeSpan time = new TimeSpan();

	[ObservableProperty]
    bool isRefreshing;

	private double _seconds;
	private IDispatcherTimer _timer;

	// Commands
	[RelayCommand]
	async Task StartTimeTracking()
	{
		var route = routingService.GetRouteByViewModel<TimesheetCreateViewModel>();
		await Navigation.NavigateTo(route);

	}

	[RelayCommand]
	async Task StopTimeTracking()
	{	
		var stopped = await timesheetService.StopActive(ActiveTimesheet.Id);
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
	async Task RefreshTimesheets()
	{	
		IsRefreshing = true;
		await GetTimeSheets();
		IsRefreshing = false;
	}

    [RelayCommand]
    async Task GetTimeSheets()
    {
		var timeheets = (await timesheetService.GetTenRecentTimesheetsAsync()).ToObservableCollection();

		RecentTimesheets.Clear();
		foreach(var timesheet in timeheets)
		{ 
			RecentTimesheets.Add(timesheet.ToTimesheetRecentListModel());
		}

    }

	[RelayCommand]
    async Task TimesheetOnTap(TimesheetRecentListModel currentTimesheet)
    {
		var x = 10;
		var z = currentTimesheet;
        
    }
	// private methods
	private async Task TryToGetActiveTimesheet()
	{ 
		var activeTimesheet = (await timesheetService.GetActive()).FirstOrDefault();
		
		if(activeTimesheet != null)
		{
			ActiveTimesheet = activeTimesheet.ToTimesheetActiveModel();
			IsTimetrackingActive = true;
			_seconds = ActiveTimesheet.Duration;
			_timer.Start();
		}
	}

	private void CreateTimer()
	{ 
		_timer = _dispatcherWrapper.CreateTimer();
		_timer.Interval = TimeSpan.FromMilliseconds(1000);
		_timer.Tick += (s, e) =>
		{
			_seconds += 1;
			Time = TimeSpan.FromSeconds(_seconds);
		};
	}
	private void timer_Tick(object sender, EventArgs e)
	{
		_seconds += 1;
	}
	

}

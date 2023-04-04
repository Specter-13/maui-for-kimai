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
using MauiForKimai.ApiClient;

namespace MauiForKimai.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
	protected readonly ITimesheetService timesheetService;
	protected readonly ILoginService _loginService;
	private readonly IDispatcherWrapper _dispatcherWrapper;
	private readonly IServerService _serverService;
    private readonly ISecureStorageService  _secureStorageService;
	

	public HomeViewModel(IRoutingService rs, 
		ILoginService ls, 
		ITimesheetService ts, 
		IDispatcherWrapper dispatcherWrapper, 
		IServerService ss, 
		ISecureStorageService sc) : base(rs, ls)
	{
		timesheetService = ts;
		_loginService = ls;
		_serverService = ss;
        _secureStorageService = sc;
		_dispatcherWrapper = dispatcherWrapper ?? new DispatcherWrapper(Application.Current.Dispatcher);
		CreateTimer();
		RegisterMessages();
	}
	// Initialize methods
	private void RegisterMessages()
	{ 
		 WeakReferenceMessenger.Default.Register<TimesheetStartMessage>(this, async (r, m) =>
        {
            SelectedActivity = m.Value.ActivityName;
			await StartTimesheet(m.Value.EditForm);
			//await TryToGetActiveTimesheet();
        });
	}

	[ObservableProperty]
	string selectedActivity;

	private async Task StartTimesheet(TimesheetEditForm form)
    { 
		await timesheetService.Create(form);
		_timer.Start();
		IsTimetrackingActive = true;
		var activeTimesheet = (await timesheetService.GetActive()).FirstOrDefault();
		ActiveTimesheet =  activeTimesheet.ToTimesheetActiveModel();
	}

	public override async Task Initialize()
    {
		var defaultServer = await _serverService.GetDefaultServer();
		if (defaultServer == null) 
		{
			return;
		}
		var key = $"{defaultServer.Id}_{defaultServer.Username}";
		//check whether this throw excpetion TODO
		var apiPassword = await _secureStorageService.Get(key);
		var defaultServerModel = (ServerModel) defaultServer;
		defaultServerModel.ApiPasswordKey = apiPassword;

		var isSuccessfull = await _loginService.LoginToDefaultOnStartUp(defaultServerModel);
		IToast toast;

		if (isSuccessfull)
		{
			toast = Toast.Make("Connection to Kimai established!", ToastDuration.Short, 14);
		    await Refresh();
		}
		else
		{
			toast = Toast.Make("Connection to Kimai failed!", ToastDuration.Short, 14);
		}
		await toast.Show();
	}

	//public override async Task OnAppearing()
	//{
	//	//IsBusy = true;
	//	// Connection to internet is available
	//	await Refresh();
	//	//IsBusy = false;
	//}


	//Properties
	[ObservableProperty]
	private TimesheetActiveModel activeTimesheet;
	public ObservableCollection<TimesheetListItemModel> RecentTimesheets {get;set; } = new();
	public ObservableCollection<TimesheetListItemGroupModel> RecentGroupedTimesheets { get; private set; } = new ObservableCollection<TimesheetListItemGroupModel>();

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
		var route = base.routingService.GetRouteByViewModel<TimesheetCreateViewModel>();
		await Navigation.NavigateTo(route,TimesheetDetailMode.Start);

	}

	[RelayCommand]
	async Task CreateNewTimesheet()
	{
		var route = base.routingService.GetRouteByViewModel<TimesheetCreateViewModel>();
		await Navigation.NavigateTo(route,TimesheetDetailMode.Create);

	}


	[RelayCommand]
	async Task StopTimeTracking()
	{	
		SelectedActivity = null;

		try
		{
			await timesheetService.StopActive(ActiveTimesheet.Id);
			await Toast.Make("Timesheet stopped successfuly!", ToastDuration.Short, 14).Show();
		}
		catch (Exception)
		{
			await Toast.Make("There was a problem to stop a timesheet! It may be already stopped.", ToastDuration.Short, 14).Show();
		}
		
		await RefreshTimesheets();
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
	async Task StartRecentTimesheet(TimesheetListItemModel timesheet)
	{	
		if(IsTimetrackingActive)
		{ 
			await Toast.Make("There is already active timesheet!", ToastDuration.Short, 14).Show();
			return;
		}

		var editForm = timesheet.ToTimesheetEditFormRegularUser();
		SelectedActivity = timesheet.ActivityName;
		editForm.Begin = new DateTimeOffset(DateTime.Now,_loginService.GetUserTimeOffset());
		await StartTimesheet(editForm);
	}

	
	[RelayCommand]
	async Task RefreshTimesheets()
	{	
		IsRefreshing = true;
		await Refresh();
		IsRefreshing = false;
	}

    [RelayCommand]
    async Task GetTimeSheets()
    {
		var timeheets = (await timesheetService.GetTenRecentTimesheetsAsync()).ToObservableCollection();

		RecentTimesheets.Clear();
		foreach(var timesheet in timeheets)
		{ 
			RecentTimesheets.Add(timesheet.ToTimesheetListItemModel());
		}

    }

  //  private Task GetTimesheestByDateGroup()
  //  {
		//var timeheets = (await timesheetService.GetTenRecentTimesheetsAsync()).ToObservableCollection();

		//RecentGroupedTimesheets.Clear();
		//var todayTimesheets = new List<TimesheetListItemModel>();
		//var olderTimesheets = new List<TimesheetListItemModel>();
		//foreach(var timesheet in timeheets)
		//{ 
		//	if(timesheet.Begin.Date == DateTime.Today)
		//	{
		//		todayGroup.Add(timesheet.ToTimesheetListItemModel());
		//	}
		//	RecentTimesheets.Add(timesheet.ToTimesheetListItemModel());
		//}

  //  }

	[RelayCommand]
    async Task TimesheetOnTap(TimesheetListItemModel currentTimesheet)
    {

		var route = base.routingService.GetRouteByViewModel<TimesheetCreateViewModel>();

		var wrapper = new TimesheetDetailWrapper(currentTimesheet,TimesheetDetailMode.Edit);
		await Navigation.NavigateTo(route,wrapper);
        
    }
	// private methods
	private async Task Refresh()
	{ 
		if (base.ApiStateProvider.IsAuthenticated && base.GetConnectivity() == NetworkAccess.Internet)
		{
			await GetTimeSheets();
			await TryToGetActiveTimesheet();
		}
		else
		{ 
			var toast = Toast.Make("Cannot acquire data!", ToastDuration.Short, 14);
			await toast.Show();
		}
	}

	private async Task TryToGetActiveTimesheet()
	{ 
		var activeTimesheet = (await timesheetService.GetActive()).FirstOrDefault();
		
		if(activeTimesheet != null)
		{
			ActiveTimesheet = activeTimesheet.ToTimesheetActiveModel();
			SelectedActivity = activeTimesheet.Activity.Name;
			IsTimetrackingActive = true;
			_seconds = ActiveTimesheet.Duration;
			_timer.Start();
		}
		else
		{
			SelectedActivity = null;
			IsTimetrackingActive = false;
			_timer.Stop();
			_seconds = 0;
			Time = TimeSpan.FromSeconds(_seconds);
			
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

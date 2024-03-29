﻿using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core.Extensions;
using System.Numerics;
using CommunityToolkit.Mvvm.Messaging;
using MauiForKimai.Messenger;
using MauiForKimai.ApiClient.Services;
using MauiForKimai.Wrappers;
using MauiForKimai.ApiClient;
using MauiForKimai.Core;
using MauiForKimai.ApiClient.Extensions;
using MauiForKimai.Popups;
using MauiForKimai.ViewModels.Settings;
using Plugin.LocalNotification;
using System.Threading.Tasks;

namespace MauiForKimai.ViewModels;

public partial class HomeViewModel : ViewModelBase, IViewModelSingleton
{
	protected readonly ITimesheetService timesheetService;
	protected readonly ILoginService _loginService;
	private readonly IDispatcherWrapper _dispatcherWrapper;
	private readonly IServerService _serverService;
    private readonly ISecureStorageService  _secureStorageService;
	

	[ObservableProperty]
	TimerWrapper  myTimer;

	[ObservableProperty]
	StatisticsWrapper  statistics;

	[ObservableProperty]
	string selectedActivity;

	[ObservableProperty]
	bool isActivityStarting;

	[ObservableProperty]
	public TimesheetActiveModel activeTimesheet;
	public ObservableCollection<TimesheetModel> RecentTimesheets {get; set; } = new ObservableCollection<TimesheetModel>();

	[ObservableProperty]
	public bool isTimetrackingActive;

	[ObservableProperty]
    bool isRefreshing;

	[ObservableProperty]
    bool isLoading;
	[ObservableProperty]
    bool showErrorLabel;
	[ObservableProperty]
	string errorText;

	private async Task ShowErrorAlert()
	{ 
		ErrorText = "Cannot acquire data!";
		ShowErrorLabel = true;
		await Task.Delay(2000);
		ShowErrorLabel = false;
	}

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

		MyTimer = new TimerWrapper(_dispatcherWrapper);
		Statistics = new StatisticsWrapper();

		RegisterMessages();
	}
	/// <summary>
	///  On viewmodel initialization, tryto login to default server and acquire data.
	/// </summary>
	public override async Task Initialize()
	{
		await ShowAlertOnFirstStartup();
		IsBusy = true;
		await LoginToDefault();
		IsBusy = false;
		if(IsTimetrackingActive)
		{
			await Refresh();
		}
	}
	/// <summary>
	///  On application resume, acquire data
	/// </summary>
	public override async Task OnApplicationResume()
	{
		await Refresh();
	}
	/// <summary>
	///  Show alert on first startup
	/// </summary>
	private async Task ShowAlertOnFirstStartup()
	{ 
		bool hasKey = Preferences.Default.ContainsKey("maui_for_kimai_first");
		if(!hasKey)
		{
			Preferences.Default.Set("maui_for_kimai_first", true);
		    await Page.DisplayAlert("MAUI for Kimai","Welcome! Create your first server in management.", "Ok");

		}
		
	}
	/// <summary>
	///  Register Messenger handlers
	/// </summary>
	private void RegisterMessages()
	{ 
		 WeakReferenceMessenger.Default.Register<TimesheetStartNewMessage>(this, async (r, m) =>
        {
            SelectedActivity = m.Value.ActivityName;
			await StartTimesheet(m.Value.Timesheet);

        });

		WeakReferenceMessenger.Default.Register<RefreshMessage>(this, async (r, m) =>
        {
            await Refresh();

        });

		 WeakReferenceMessenger.Default.Register<TimesheetStartExistingMessage>(this, async (r, m) =>
        {
            SelectedActivity = m.Value.ActivityName;
			await StartTimesheet(m.Value);
        });
	}

	
	/// <summary>
	///  Start the timesheet provided by Timesheet model,
	/// </summary>
	/// <param name="model">TimesheetModel, which application will try to start.</param>
	private async Task StartTimesheet(TimesheetModel model)
    { 
		if(IsTimetrackingActive)
		{ 
			await Toast.Make("There is already active timesheet!", ToastDuration.Short, 14).Show();
			return;
		}

		try
		{
			IsActivityStarting = true;
			var form = model.ToTimesheetEditForm(LoginContext.TimetrackingPermissions, LoginContext.TimeOffset);
			var timesheet = await timesheetService.Create(form);

			if(model.GitlabIssueId != null) 
			{
				var meta = new Body5();
				meta.Name ="gitlab_issue_id";
				meta.Value = model.GitlabIssueId.Value.ToString();
				await timesheetService.SetMetaField(timesheet.Id.Value, meta);
			}
			if(timesheet == null) 
			{
				await Toast.Make("Error starting timesheet! Check your connection.", ToastDuration.Long, 14).Show();
			}
			else
			{
				IsTimetrackingActive = true;
				var activeTimesheet = (await timesheetService.GetActive()).FirstOrDefault();
				if(activeTimesheet == null) 
				{
					await Toast.Make($"Cannot start timesheet!", ToastDuration.Short, 14).Show();
					return;
				}
				ActiveTimesheet =  activeTimesheet.ToTimesheetActiveModel();
				SelectedActivity = ActiveTimesheet.ActivityName;
				MyTimer.TimerStartExisting(ActiveTimesheet.Duration);
				await Toast.Make($"Activity {SelectedActivity} started successfully", ToastDuration.Short, 14).Show();
				#if ANDROID || IOS
				await TryToCreateNotification();
				#endif
			}
			IsActivityStarting = false;
			
		}
		catch (KimaiApiException)
		{
			IsActivityStarting = false;
			IsRefreshing = false;
			await Toast.Make("Error starting timesheet!", ToastDuration.Long, 14).Show();
			return;
		}
		
	}

	/// <summary>
	///  Go to TimesheedDetail view for craeting new timesheet to start.
	/// </summary>

	[RelayCommand]
	async Task StartNewTimesheet()
	{
		var route = base.routingService.GetRouteByViewModel<TimesheetDetailViewModel>();
		await Navigation.NavigateTo(route,TimesheetDetailMode.Start);
	}

	/// <summary>
	///  Start recent timesheet.
	/// </summary>
	/// <param name="timesheet">Timesheet to start</param>
	[RelayCommand]
	async Task StartRecentTimesheet(TimesheetModel timesheet)
	{	
		timesheet.Begin = DateTime.Now;
		timesheet.End = null;
		await StartTimesheet(timesheet);
	}
	/// <summary>
	///  Go to TimesheedDetail view for craeting new timesheet.
	/// </summary>
	[RelayCommand]
	async Task CreateNewTimesheet()
	{
		var route = base.routingService.GetRouteByViewModel<TimesheetDetailViewModel>();
		await Navigation.NavigateTo(route,TimesheetDetailMode.Create);

	}
	/// <summary>
	///  Stop active time-tracking.
	/// </summary>
	[RelayCommand]
	async Task StopTimeTracking()
	{	
		if(HasInternetAndIsLogged())
		{

			TimesheetEntity stopped;
			try
			{
				stopped = await timesheetService.StopActive(ActiveTimesheet.Id);
			}
			catch (KimaiApiException)
			{
				stopped = null;
			}
			
			if(stopped == null)
			{
				await Toast.Make("There was a problem to stop a timesheet! It may be already stopped or time offset is wrong!", ToastDuration.Long, 14).Show();
				#if ANDROID || IOS
				TryToStopNotification();
					#endif
				return;
			}

			IsTimetrackingActive = false;
			SelectedActivity = null;
			MyTimer.TimerStop();
			await RefreshTimesheets();
			#if ANDROID || IOS
			TryToStopNotification();
			#endif
			await Toast.Make("Timesheet stopped successfuly!", ToastDuration.Short, 14).Show();
				
			
		}
		else
		{
			await Toast.Make("Error stopping timesheet. Check your internet connection.", ToastDuration.Long, 14).Show();
		}
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
		await Refresh();
		IsRefreshing = false;
	}

    [RelayCommand]
    async Task GetRecentTimesheets()
    {
		var timesheets = await timesheetService.GetTenRecentTimesheetsAsync();
		RecentTimesheets.Clear();
		if(timesheets == null) return;

		foreach (var timesheet in timesheets)
		{
			var model = timesheet.ToTimesheetModel();
			model.IsRecent = true;
			RecentTimesheets.Add(model);
		}

	}


	[RelayCommand]
    async Task ShowDetail(TimesheetModel currentTimesheet)
    {
		var route = base.routingService.GetRouteByViewModel<TimesheetDetailViewModel>();
		var wrapper = new TimesheetDetailWrapper(currentTimesheet,TimesheetDetailMode.Edit);
		await Navigation.NavigateTo(route,wrapper);
    }

	[RelayCommand]
    async Task GoToSettings()
    {
		var route = base.routingService.GetRouteByViewModel<SettingsViewModel>();
		await Navigation.NavigateTo(route);
    }

	// private methods
	private async Task Refresh()
	{ 

		if (HasInternetAndIsLogged())
		{
			try
			{
				IsLoading = true;
				await GetRecentTimesheets();
				await TryToGetActiveTimesheet();
				await CalculateTodayStatistics();
				IsLoading = false;

			}
			catch (KimaiApiException)
            {
				Statistics = new StatisticsWrapper();
				await Toast.Make("Cannot acquire data!", ToastDuration.Short, 14).Show();
			}
			
		}
		else
		{ 
			#if ANDROID || IOS
			TryToStopNotification();
			#endif 
			Statistics = new StatisticsWrapper();
			RecentTimesheets.Clear();
			IsTimetrackingActive = false;
			await ShowErrorAlert();
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
			MyTimer.TimerStartExisting(ActiveTimesheet.Duration);
			#if ANDROID || IOS
			await TryToCreateNotification();
			#endif 
		}
		else
		{
			#if ANDROID || IOS
			TryToStopNotification();
			#endif 

			SelectedActivity = null;
			IsTimetrackingActive = false;
			MyTimer.TimerStop();
			
		}
	}



	private async Task CalculateTodayStatistics()
	{ 
		var todayTimesheet = await timesheetService.GetTodayTimesheetsAsync();
		if(todayTimesheet  == null) return;
		await Statistics.CalculateTodayStatistics(todayTimesheet);
		OnPropertyChanged(nameof(Statistics));
	}


	public async Task LoginToDefault()
	{ 
		var defaultServer = await _serverService.GetDefaultServer();
		if (defaultServer == null) 
		{
			return;
		}
		var key = $"{defaultServer.Id}_{defaultServer.Username}";
		//check whether this throw excpetion TODO
		var apiPassword = await _secureStorageService.Get(key);
		var defaultServerModel = defaultServer.ToServerModel();
		defaultServerModel.ApiPasswordKey = apiPassword;

		var isSuccessfull = await _loginService.LoginOnStartUp(defaultServerModel);
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

	#if ANDROID || IOS
		async Task TryToCreateNotification()
		{ 
		
			if (await LocalNotificationCenter.Current.AreNotificationsEnabled() == false)
			{
				await LocalNotificationCenter.Current.RequestNotificationPermission();
			}

			var notification = new NotificationRequest
			{
				NotificationId = 100,
				Title = $"{ActiveTimesheet?.Start.TimeOfDay.ToString(@"hh\:mm")} Time-tracking is active ",
				Description = $"{ActiveTimesheet.ActivityName}, {ActiveTimesheet.ProjectName}",
				CategoryType = NotificationCategoryType.Service,
				Silent = true,
				Android =
				{
					AutoCancel = false,
					Ongoing = true,   
				}
			};
			await LocalNotificationCenter.Current.Show(notification);
				
	
		}
		void TryToStopNotification()
		{ 
			LocalNotificationCenter.Current.Cancel(100);
		
		}
	#endif


}

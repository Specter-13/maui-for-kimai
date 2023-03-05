using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiForKimai.ApiClient.Authentication;
using MauiForKimai.ViewModels.Base;
using MauiForKimai.ApiClient.Interfaces;
using CommunityToolkit.Mvvm.Messaging;
using MauiForKimai.ApplicationLayer.Messages;
using MauiForKimai.ApiClient.ApiClient;
using MauiForKimai.ApiClient.Client;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core.Extensions;

namespace MauiForKimai.ViewModels;

public partial class MainViewModel : ViewModelBase
{
	protected readonly ITimesheetService timesheetService;

	public MainViewModel(ITimesheetService ts, ApiStateProvider asp) : base(asp)
	{
		timesheetService = ts;

		
	}
	
	

	public ObservableCollection<TimesheetCollectionExpanded> RecentTimesheets {get;set; } = new();


    [RelayCommand]
    async System.Threading.Tasks.Task GetTimeSheets()
    {
		RecentTimesheets = (await timesheetService.GetTenRecentTimesheetsAsync()).ToObservableCollection();
		string text = "Logged!";
		ToastDuration duration = ToastDuration.Short;
		double fontSize = 14;

		var toast = Toast.Make(text, duration, fontSize);
		CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
		await toast.Show(cancellationTokenSource.Token);
        
    }
}


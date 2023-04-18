using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Messaging;
using MauiForKimai.ApiClient;
using MauiForKimai.ApiClient.Authentication;
using MauiForKimai.Messenger;
using MauiForKimai.ViewModels.Base;
using MauiForKimai.Wrappers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ViewModels;
public partial class TimesheetListViewModel : ViewModelBase, IViewModelSingleton
{
    private readonly ITimesheetService _timesheetService;
    public TimesheetListViewModel(IRoutingService rs, ILoginService ls, ITimesheetService ts) : base(rs, ls)
    {
        _timesheetService = ts;
        WeakReferenceMessenger.Default.Register<RefreshMessage>(this, async (r, m) =>
        {
			await Refresh();
        });
    }
    
    [ObservableProperty]
    private bool isRefreshing;
    public override async Task Initialize()
    {
        IsBusy = true;
        page = 1;
        await GetTimesheetsIncrementaly();
        IsBusy = false;
    }

    [RelayCommand]
    async Task Refresh()
    { 
        IsBusy = true;
        page = 1;
       Timesheets.Clear();
       await GetTimesheetsIncrementaly();
        IsBusy = false;
    }

    public ObservableCollection<TimesheetModel> Timesheets { get; set;} = new();

    private int page = 1;

    [ObservableProperty]
    int numberOfEntries;

    int numberOfFechted = 10;

    [ObservableProperty]
    private bool isLoadingMore;

    private bool isFullyLoaded;
    [RelayCommand]
    async Task LoadMore()
    {
         if (isFullyLoaded)
            return;

      
        page += 1;
        try
        {
            IsLoadingMore = true;
            await GetTimesheetsIncrementaly();
            IsLoadingMore = false;
            
        }
        catch (KimaiApiException)
        {
            isFullyLoaded = true;
            IsLoadingMore = false;
        }
       
        

    }
    [RelayCommand]
    async Task ShowDetail(TimesheetModel model)
    {
        var wrapper = new TimesheetDetailWrapper(model,TimesheetDetailMode.Edit);
        var route = base.routingService.GetRouteByViewModel<TimesheetDetailAllViewModel>();
		await Navigation.NavigateTo(route,wrapper);
    }

    [RelayCommand]
    async Task QuickStart(TimesheetModel model)
    {
        model.Begin =  DateTime.Now;
        model.End =  null;
        WeakReferenceMessenger.Default.Send(new TimesheetStartExistingMessage(model));
        var route = base.routingService.GetRouteByViewModel<HomeViewModel>();
		await Navigation.NavigateTo(route, model);
    }



    private async Task GetTimesheetsIncrementaly()
    { 

        if(HasInternetAndIsLogged())
        { 
            var timesheets = await _timesheetService.GetTimesheetsIncrementalyAsync(page,numberOfFechted);
            foreach (var item in timesheets)
            {
                Timesheets.Add(item.ToTimesheetModel());
                NumberOfEntries += 1;
            }
        }
        else
		{ 
			var toast = Toast.Make("Cannot acquire data!", ToastDuration.Short, 14);
			await toast.Show();
		}
     }
}

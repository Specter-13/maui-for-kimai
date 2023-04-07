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
public partial class TimesheetListViewModel : ViewModelBase
{
    private readonly ITimesheetService _timesheetService;
    public TimesheetListViewModel(IRoutingService rs, ILoginService ls, ITimesheetService ts) : base(rs, ls)
    {
        _timesheetService = ts;

    }
    
    [ObservableProperty]
    private bool isRefreshing;
    public override async Task Initialize()
    {
        IsBusy = true;
        await LoadMore();
        IsBusy = false;
        //return base.OnAppearing();
    }

    [RelayCommand]
    async Task Refresh()
    { 
       IsRefreshing = true;
       page = 1;
       Timesheets.Clear();
       await GetTimesheetsIncrementaly();
        IsRefreshing = false;
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
        catch (KiamiApiException)
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
        model.Begin =  new DateTimeOffset(DateTime.Now, loginService.GetUserTimeOffset());
        model.End =  null;
        WeakReferenceMessenger.Default.Send(new TimesheetStartExistingMessage(model));
        var route = base.routingService.GetRouteByViewModel<HomeViewModel>();
		await Navigation.NavigateTo(route, model);
    }



    private async Task GetTimesheetsIncrementaly()
    { 
        var timesheets = await _timesheetService.GetTimesheetsIncrementalyAsync(page,numberOfFechted);
        foreach (var item in timesheets)
        {
            Timesheets.Add(item.ToTimesheetModel());
            NumberOfEntries += 1;
        }
     }
}

using MauiForKimai.ApiClient;
using MauiForKimai.ApiClient.Authentication;
using MauiForKimai.ViewModels.Base;
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
        await LoadMore();
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

    public ObservableCollection<TimesheetListItemModel> Timesheets { get; set;} = new();

    private int page = 1;

    [ObservableProperty]
    int numberOfEntries;

    int numberOfFechted = 10;

    private bool isFullyLoaded;
    [RelayCommand]
    async Task LoadMore()
    {
         if (IsBusy || isFullyLoaded)
            return;

        IsBusy = true;
        page += 1;
        try
        {
            await GetTimesheetsIncrementaly();
            
        }
        catch (KiamiApiException)
        {
            isFullyLoaded = true;
        }
       
        IsBusy = false;

    }

    private async Task GetTimesheetsIncrementaly()
    { 
        var timesheets = await _timesheetService.GetTimesheetsIncrementalyAsync(page,numberOfFechted);
        foreach (var item in timesheets)
        {
            Timesheets.Add(item.ToTimesheetListItemModel());
            NumberOfEntries += 1;
        }
     }
}

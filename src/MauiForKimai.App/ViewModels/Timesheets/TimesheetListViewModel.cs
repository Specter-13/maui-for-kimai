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

    //public override async Task OnAppearing()
    //{
    //    IsBusy = true;
    //    var timesheets = await _timesheetService.GetTimesheetsIncrementalyAsync(page, 20);

    //    Timesheets.Clear();
    //    foreach (var item in timesheets)
    //    {
    //        Timesheets.Add(item);
    //    }
    //    page = 1;
    //    IsBusy = false;
    //    //return base.OnAppearing();
    //}

    public ObservableCollection<TimesheetCollectionExpanded> Timesheets { get; set;} = new();

    private int page = 0;
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
            var timesheets = await _timesheetService.GetTimesheetsIncrementalyAsync(page,20);
            foreach (var item in timesheets)
            {
                Timesheets.Add(item);
            }
        }
        catch (KiamiApiException)
        {
            isFullyLoaded = true;
        }
       
        IsBusy = false;

    }
}

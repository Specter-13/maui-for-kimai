using MauiForKimai.Views.Timesheets;
using Mopups.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ViewModels.Timesheets;

public partial class TimesheetCreateViewModel : ViewModelBase
{
    private readonly IPopupNavigation _mopup;
    public TimesheetCreateViewModel(ApiStateProvider asp, IRoutingService routingService, IPopupNavigation mopup) : base(asp, routingService)
    {
        _mopup = mopup;
    }

    [ObservableProperty]
    TimesheetEditForm actualTimesheet;

    [ObservableProperty]
    string duration;

    [ObservableProperty]
    string beginDateString;

    [ObservableProperty]
    DateTime beginDate;

    [ObservableProperty]
    DateTime endDate;

    [ObservableProperty]
    TimeSpan beginTime;

    [ObservableProperty]
    TimeSpan endTime;


    public override async Task OnParameterSet()
    {
        ActualTimesheet = NavigationParameter as TimesheetEditForm;
        


        BeginTime = ActualTimesheet.Begin.TimeOfDay;
        EndTime = ActualTimesheet.End.Value.TimeOfDay;

        BeginDate = ActualTimesheet.Begin.Date;
        EndDate = ActualTimesheet.End.Value.Date;

        Duration = (EndTime - BeginTime).ToString(@"hh\:mm\:ss");
        BeginDateString = BeginDate.ToString("dddd, dd MMMM yyyy");

    }


    [RelayCommand]
    async Task ShowProjectMopup()
    {
        await _mopup.PushAsync(new TimesheetProjectChooseMopupView());
    }



}

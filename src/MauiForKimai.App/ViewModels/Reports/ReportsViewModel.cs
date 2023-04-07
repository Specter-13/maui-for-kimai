using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Messaging;
using MauiForKimai.ApiClient;
using MauiForKimai.Messenger;
using MauiForKimai.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyMvvm;

namespace MauiForKimai.ViewModels;


public partial class ReportsViewModel : TinyViewModel
{
    private readonly ITimesheetService _timesheetService;
    public ReportsViewModel(IRoutingService rs, ILoginService ls, ITimesheetService ts)
    {
        _timesheetService = ts;
    }

    public override async Task Initialize()
    {
        IsBusy = true;
        await GetData();
        IsBusy = false;
    }

    [RelayCommand]
    public async Task Refresh()
    {
        if(Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
        { 
            
            MonthData.Clear();
            WeekData.Clear();
            TodayData.Clear();
            await GetData();
            IsRefreshing = false;
        }
        else
        {
           await Toast.Make("Cannot acquire report data", ToastDuration.Short, 14).Show();
        }
    }

    [ObservableProperty]
    bool isDataLoaded;

    [ObservableProperty]
    bool isRefreshing;

    private Task CalculateProjectData(DateTime now)
    {
        return Task.Run(() =>
        { 
            var firstDayOfCurrentWeek = now.StartOfWeek(DayOfWeek.Monday);
            var lastDayOfWeek = firstDayOfCurrentWeek.AddDays(7);
            //get only timesheets of current week;
            var thisWeekTimesheets = _timesheets.Where(p =>  firstDayOfCurrentWeek.Date <= p.Begin.Date &&  p.Begin.Date <= lastDayOfWeek);
        
            //get only today timesheets;
            var todayTimesheets = _timesheets.Where(p => p.Begin.Date == now.Date);


            FillGraphDataAsync(todayTimesheets,TodayData,ReportsType.Project);
            FillGraphDataAsync(thisWeekTimesheets, WeekData,ReportsType.Project);
            FillGraphDataAsync(_timesheets, MonthData,ReportsType.Project);


            FillGraphDataAsync(todayTimesheets,TodayCustomerData,ReportsType.Customer);
            FillGraphDataAsync(thisWeekTimesheets, WeekCustomerData,ReportsType.Customer);
            FillGraphDataAsync(_timesheets, MonthCustomerData,ReportsType.Customer);

            FillGraphDataAsync(todayTimesheets,TodayActivityData,ReportsType.Activity);
            FillGraphDataAsync(thisWeekTimesheets, WeekActivityData,ReportsType.Activity);
            FillGraphDataAsync(_timesheets, MonthActivityData,ReportsType.Activity);

        });
    }


    public async Task GetData()
    {
        IsDataLoaded = false;
        var now = DateTime.Now;
        var lastDayOfMonth = DateTime.DaysInMonth(now.Year, now.Month);

        // first day of a month
        var beginTime = new DateTime(now.Year, now.Month, 1);
        var endTime = new DateTime(now.Year, now.Month, lastDayOfMonth);

        //get timesheets of current month
        _timesheets = await _timesheetService.GetTimesheetsForReportsAsync(beginTime.ToRFC3339() ,endTime.ToRFC3339());

        if(_timesheets == null || _timesheets.Count() == 0)
        {
            //if cannot acquire data for a month
            //handle error 
            
        }

        await CalculateProjectData(now);
        WeakReferenceMessenger.Default.Send(new ChartLoadMessage(""));
        IsDataLoaded = true;
    }

    private Task FillGraphDataAsync(IEnumerable<TimesheetCollectionExpanded> timesheets, Dictionary<string,float> data, ReportsType reportType)
    { 
        return Task.Run(() =>
        { 
            var barChartWidth = 10;
        
            foreach (var timesheet in timesheets) 
            {
                var duration = timesheet.Duration;
                if(duration == null) continue;

                var name = GetKeyByReportsType(reportType,timesheet);
               
                var isSuccess = data.ContainsKey(name);

                //project is not in dictionary, add it there and add width
                if(!isSuccess)
                {
                    data.Add(name, (float)duration);
                    barChartWidth += 40;
                }
                else
                { 
                    //if project is in dictionary, update duration
                    data[name] +=  (float)duration;
                }
            }
        });

    }

    private string GetKeyByReportsType(ReportsType reportType, TimesheetCollectionExpanded timesheet)
    { 
        if(reportType == ReportsType.Project)
        { 
            return timesheet.Project.Name;
        }
        else if(reportType == ReportsType.Customer)
        {
            return timesheet.Project.Customer.Name; 
        }
        else
        {
            return timesheet.Activity.Name;
        }
     }

    private IEnumerable<TimesheetCollectionExpanded> _timesheets;

    [ObservableProperty]
    public Dictionary<string,float> monthData  = new Dictionary<string, float>{{"",0}};
    [ObservableProperty]
    public Dictionary<string,float> weekData = new Dictionary<string, float>{{"",0}};
    [ObservableProperty]
    public Dictionary<string,float> todayData = new Dictionary<string, float>{{"",0}};


    [ObservableProperty]
    public Dictionary<string,float> monthCustomerData  = new Dictionary<string, float>{{"",0}};
    [ObservableProperty]
    public Dictionary<string,float> weekCustomerData = new Dictionary<string, float>{{"",0}};
    [ObservableProperty]
    public Dictionary<string,float> todayCustomerData = new Dictionary<string, float>{{"",0}};


     [ObservableProperty]
    public Dictionary<string,float> monthActivityData  = new Dictionary<string, float>{{"",0}};
    [ObservableProperty]
    public Dictionary<string,float> weekActivityData = new Dictionary<string, float>{{"",0}};
    [ObservableProperty]
    public Dictionary<string,float> todayActivityData = new Dictionary<string, float>{{"",0}};

    [ObservableProperty]
    public int monthDataWidth;
    [ObservableProperty]
    public int weekDataWidth;
    [ObservableProperty]
    public int todayDataWidth;


}


public static class DateTimeExtensions
{
     public static string ToRFC3339(this DateTime date)
     {
         return date.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss");
     }

    public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
    {
        int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
        return dt.AddDays(-1 * diff).Date;
    }
}
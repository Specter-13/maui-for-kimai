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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MauiForKimai.ViewModels;


public partial class ReportsViewModel : ViewModelBase
{
    private readonly ITimesheetService _timesheetService;
    public ReportsViewModel(IRoutingService rs, ILoginService ls, ITimesheetService ts) : base(rs, ls)
    {

        _timesheetService = ts;

    }

    public override async Task Initialize()
    {
        if(base.GetConnectivity() == NetworkAccess.Internet)
        { 
            await GetData();
        }
        else
        {
           await Toast.Make("Cannot acquire report data", ToastDuration.Short, 14).Show();
        }
        //return base.Initialize();
    }

    [RelayCommand]
    public async Task Refresh()
    {
        MonthData.Clear();
        WeekData.Clear();
        TodayData.Clear();
        await GetData();
    }

    [ObservableProperty]

    bool isDataLoaded;

    public async Task GetData()
    {
        //dd-mm-yyyy
        //2019-04-20T14:00:00

        IsBusy = true;
        var now = DateTime.Now;
        var lastDayOfMonth = DateTime.DaysInMonth(now.Year, now.Month);
        // first day of a month
        var beginTime = new DateTime(now.Year, now.Month, 1);
        var endTime = new DateTime(now.Year, now.Month, lastDayOfMonth);
        //"2023-03-24T00:00:00"

        //get timesheets of current month
        var timesheets = await _timesheetService.GetTimesheetsForReportsAsync(beginTime.ToRFC3339() ,endTime.ToRFC3339());

        if(timesheets == null || timesheets.Count == 0)
        {
            //if cannot acquire data for a month
            //handle error 
            
        }

        var firstDayOfCurrentWeek = now.StartOfWeek(DayOfWeek.Monday);
        var lastDayOfWeek = firstDayOfCurrentWeek.AddDays(7);
        //get only timesheets of current week;
        var thisWeekTimesheets = timesheets.Where(p =>  firstDayOfCurrentWeek.Date <= p.Begin.Date &&  p.Begin.Date <= lastDayOfWeek);
        
        //get only today timesheets;
        var todayTimesheets = timesheets.Where(p => p.Begin.Date == now.Date);


        var todayWrapper = FillGraphData(todayTimesheets,TodayData);
        TodayMaxValue = todayWrapper.Max;
        var weekWrapper  = FillGraphData(thisWeekTimesheets,WeekData);
        WeekMaxValue = weekWrapper.Max;
        var monthWrapper  = FillGraphData(timesheets,MonthData);
        MonthMaxValue = monthWrapper.Max;


        IsBusy = false;
        
        
        WeakReferenceMessenger.Default.Send(new TodayWeekMonthWrapper(todayWrapper,weekWrapper,monthWrapper));
        IsDataLoaded = true;
       //var barChartWrapper = new BarChartDataWrapper(TodayData, TodayDataWidth);
        //WeakReferenceMessenger.Default.Send(new ChartLoadMessage(barChartWrapper));
    }

    private BarChartDataWrapper FillGraphData(IEnumerable<TimesheetCollectionExpanded> timesheets, Dictionary<string,float> data)
    { 
        var barChartWidth = 10;
        foreach (var timesheet in timesheets) 
        {
            var projectName = timesheet.Project.Name;
            var duration = (float)timesheet.Duration;
            var isSuccess = data.ContainsKey(projectName);

            //project is not in dictionary, add it there and add width
            if(!isSuccess)
            {
                data.Add(projectName, duration);
                barChartWidth += 40;
            }
            else
            { 
                //if project is in dictionary, update duration
                data[projectName] +=  (float)timesheet.Duration;
            }
            
        }

        return new BarChartDataWrapper(data, barChartWidth, data.Values.Max());

    }
    [ObservableProperty]
    public Dictionary<string,float> monthData  = new Dictionary<string, float>();
    [ObservableProperty]
    public Dictionary<string,float> weekData = new Dictionary<string, float>();
    [ObservableProperty]
    public Dictionary<string,float> todayData = new Dictionary<string, float>();

    [ObservableProperty]
    public int monthDataWidth;
    [ObservableProperty]
    public int weekDataWidth;
    [ObservableProperty]
    public int todayDataWidth;

    [ObservableProperty]
    public float monthMaxValue;
    [ObservableProperty]
    public float weekMaxValue;
    [ObservableProperty]
    public float todayMaxValue;
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
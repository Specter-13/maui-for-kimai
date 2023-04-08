using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Messaging;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using MauiForKimai.ApiClient;
using MauiForKimai.Messenger;
using MauiForKimai.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyMvvm;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using LiveChartsCore.Defaults;
using System.Collections.ObjectModel;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using Newtonsoft.Json.Linq;
using MauiForKimai.Core.Enums;

namespace MauiForKimai.ViewModels;


public partial class ReportsViewModel : ViewModelBase
{
    private const int _chartWidth = 150;
    private readonly ITimesheetService _timesheetService;
    public ReportsViewModel(IRoutingService rs, ILoginService ls, ITimesheetService ts) : base(rs,ls)
    {
        _timesheetService = ts;


    

    }

    private StatisticsWrapper statisticsWrapper = new StatisticsWrapper("ahoj");
    private static double _textSize =  DeviceInfo.Current.Idiom == DeviceIdiom.Desktop ? 15 : 40;

    public override async Task Initialize()
    {
        if(Connectivity.Current.NetworkAccess == NetworkAccess.Internet && base.ApiStateProvider.IsAuthenticated )
        { 
            IsBusy = true;
            await GetData();
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task Refresh()
    {
        if(Connectivity.Current.NetworkAccess == NetworkAccess.Internet && base.ApiStateProvider.IsAuthenticated )
        { 
            

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

    [ObservableProperty]
    string pageLabel;

    [ObservableProperty]
    int selectedReportType;

    partial void OnSelectedReportTypeChanged(int value)
    {
        GetDataByReportsType((ReportsType)value);
    }

    private IEnumerable<TimesheetCollectionExpanded> _monthTimesheets;
    private IEnumerable<TimesheetCollectionExpanded> _weekTimesheets;
    private IEnumerable<TimesheetCollectionExpanded> _todayTimesheets;

    public ObservableCollection<ISeries> TodaySeries { get; set; }

    public ObservableCollection<Axis> TodayXAxes { get; set; } = new ()
    {
        new Axis
        {
            TextSize = _textSize
        }
    };

    [ObservableProperty]
    public int todayChartWidth = _chartWidth;

     public ObservableCollection<ISeries> WeekSeries { get; set; }

    public ObservableCollection<Axis> WeekXAxes { get; set; } = new ()
    {
        new Axis
        {
            TextSize = _textSize
        }
    };
    [ObservableProperty]
    public int weekChartWidth = _chartWidth;

    public ObservableCollection<ISeries> MonthSeries { get; set; }

    public ObservableCollection<Axis> MonthXAxes { get; set; } = new ()
    {
        new Axis
        {
            TextSize = _textSize
        }
    };
    [ObservableProperty]
    public int monthChartWidth = _chartWidth;

     public ObservableCollection<Axis> CommonYAxes { get; set; } = new ()
    {
        new Axis
        {
            TextSize = _textSize,
            Labeler = (value) => ConvertDurationToFormattedString((int)value)
        }
    };
  

    [RelayCommand]
    async Task GetDataByReportsType(ReportsType reportsType)
    {

        var wrapper = await CreateGraphAsync(_todayTimesheets, reportsType);
        TodaySeries = wrapper.Series;
        TodayChartWidth = _chartWidth;
        TodayChartWidth += wrapper.ChartWidth;
        TodayXAxes.First().Labels = wrapper.Labels;
        OnPropertyChanged(nameof(TodaySeries));

        wrapper = await CreateGraphAsync(_weekTimesheets, reportsType);
        WeekSeries = wrapper.Series;
        WeekChartWidth = _chartWidth;
        WeekChartWidth += wrapper.ChartWidth;
        WeekXAxes.First().Labels = wrapper.Labels;
        OnPropertyChanged(nameof(WeekSeries));

        wrapper = await CreateGraphAsync(_monthTimesheets, reportsType);
        MonthSeries = wrapper.Series;
        MonthChartWidth = _chartWidth;
        MonthChartWidth += wrapper.ChartWidth;
        MonthXAxes.First().Labels = wrapper.Labels;
        OnPropertyChanged(nameof(MonthSeries));
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
        _monthTimesheets = await _timesheetService.GetTimesheetsForReportsAsync(beginTime.ToRFC3339() ,endTime.ToRFC3339());
        
        if(_monthTimesheets == null || _monthTimesheets.Count() == 0)
        {
            //if cannot acquire data for a month
            //handle error 
            return;
        }

        var firstDayOfCurrentWeek = now.StartOfWeek(DayOfWeek.Monday);
        var lastDayOfWeek = firstDayOfCurrentWeek.AddDays(7);

        _weekTimesheets = _monthTimesheets.Where(p => firstDayOfCurrentWeek.Date <= p.Begin.Date && p.Begin.Date <= lastDayOfWeek);
        _todayTimesheets = _monthTimesheets.Where(p => p.Begin.Date == now.Date);


        await GetDataByReportsType(ReportsType.Project);
        SelectedReportType = 0;
        IsDataLoaded = true;
    }

    private async Task<ChartSeriesWrapper> CreateGraphAsync(IEnumerable<TimesheetCollectionExpanded> timesheets, ReportsType reportsType)
    { 
        //initialize graph
        var mySeries = new ObservableCollection<ISeries>();
        var columnSeries = new ColumnSeries<int>
        {
            Name = reportsType.ToString(),
            Fill = new SolidColorPaint(SKColors.CornflowerBlue),
            MaxBarWidth = 100,
            TooltipLabelFormatter =
                (chartPoint) => $"Duration: {ConvertDurationToFormattedString((int)chartPoint.PrimaryValue)}",
        };

        // calculate data
        var wrapper = await FillGraphDataAsync(timesheets,reportsType);

        //add data to graph
        columnSeries.Values = wrapper.Data.Values;

        // add x axis labels
        var labels = new Axis
        {
            Labels = wrapper.Data.Keys.ToList(),
            TextSize = _textSize,
        };

        mySeries.Add(columnSeries);

        return new ChartSeriesWrapper(mySeries,wrapper.Width, wrapper.Data.Keys.ToList());
    }

    

    private Task<ChartDataWrapper> FillGraphDataAsync(IEnumerable<TimesheetCollectionExpanded> timesheets, ReportsType reportType)
    { 
        return Task.Run(() =>
        { 
            var data  = new Dictionary<string, int>();
            var width = 0;
            foreach (var timesheet in timesheets) 
            {
                var duration = timesheet.Duration;
                if(duration == null) continue;
                var name = GetKeyByReportsType(reportType,timesheet);
                var splitted = SplitLongStrings(name);
                var isSuccess = data.ContainsKey(splitted);
                //project is not in dictionary, add it there and add width
                if(!isSuccess)
                {
                    data.Add(splitted, duration.Value);
                    if(DeviceInfo.Current.Idiom == DeviceIdiom.Desktop)
                        width += 180;
                    else
                        width += 100;
                }
                else
                { 
                    //if project is in dictionary, update duration
                    data[splitted] +=  duration.Value;
                }
            }
            
        
            //return data;
            return new ChartDataWrapper(data, width);

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

    private string SplitLongStrings(string name)
    { 
        string[] words = name.Split(' ');
        StringBuilder sb = new StringBuilder();
        int currLength = 0;
        foreach(string word in words)
        {
            if(currLength + word.Length + 1 < 15) // +1 accounts for adding a space
            {
              sb.AppendFormat(" {0}", word);
              currLength = (sb.Length % 15);
            }
            else
            {
              sb.AppendFormat("{0} {1}", Environment.NewLine, word);
              currLength = 0;
            }
        }

        return sb.ToString();
    }

    private static string ConvertDurationToFormattedString(int value)
    {
        var duration = TimeSpan.FromSeconds(value);
        return $"{((int)duration.TotalHours).ToString("00")}:{duration.Minutes.ToString("00")}";
    }

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


   //private Task CalculateProjectData(DateTime now)
    //{
    //    return Task.Run(() =>
    //    { 
    //        var firstDayOfCurrentWeek = now.StartOfWeek(DayOfWeek.Monday);
    //        var lastDayOfWeek = firstDayOfCurrentWeek.AddDays(7);
    //        //get only timesheets of current week;
    //        var thisWeekTimesheets = _timesheets.Where(p =>  firstDayOfCurrentWeek.Date <= p.Begin.Date &&  p.Begin.Date <= lastDayOfWeek);
        
    //        //get only today timesheets;
    //        var todayTimesheets = _timesheets.Where(p => p.Begin.Date == now.Date);


    //        FillGraphDataAsync(todayTimesheets,TodayData,ReportsType.Project);
    //        FillGraphDataAsync(thisWeekTimesheets, WeekData,ReportsType.Project);
    //        FillGraphDataAsync(_timesheets, MonthData,ReportsType.Project);


    //        FillGraphDataAsync(todayTimesheets,TodayCustomerData,ReportsType.Customer);
    //        FillGraphDataAsync(thisWeekTimesheets, WeekCustomerData,ReportsType.Customer);
    //        FillGraphDataAsync(_timesheets, MonthCustomerData,ReportsType.Customer);

    //        FillGraphDataAsync(todayTimesheets,TodayActivityData,ReportsType.Activity);
    //        FillGraphDataAsync(thisWeekTimesheets, WeekActivityData,ReportsType.Activity);
    //        FillGraphDataAsync(_timesheets, MonthActivityData,ReportsType.Activity);

    //    });
    //}
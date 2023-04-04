using CommunityToolkit.Mvvm.Messaging;
using MauiForKimai.Messenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ViewModels;

public class ReportsViewModel : ViewModelBase
{
    private readonly ITimesheetService _timesheetService;
    public ReportsViewModel(IRoutingService rs, ILoginService ls, ITimesheetService ts) : base(rs, ls)
    {

        _timesheetService = ts;

    }

    public override async Task Initialize()
    {
        await GetData();
        //return base.Initialize();
    }
    private async Task GetData()
    {
        //dd-mm-yyyy
        //2019-04-20T14:00:00
        IsBusy = true;
        var d = DateTime.Now;
        var now = DateTime.Now.ToRFC3339();
        
        var z= 10;
        var timesheets = await _timesheetService.GetTimesheetsForReportsAsync("2023-03-24T00:00:00",now);
        var barChartWidth = 30;
        foreach (var timesheet in timesheets) 
        {
            var projectName = timesheet.Project.Name;

            
            var duration = (float)timesheet.Duration;
            
            var isSuccess = Data.ContainsKey(projectName);

            //project is not in dictionary, add it there
            if(!isSuccess)
            {
                Data.Add(projectName, duration);
                barChartWidth += 40;
            }
            else
            { 
                //if project is in dictionary, update duration
                Data[projectName] +=  (float)timesheet.Duration;
            }
            
        }

 
        IsBusy = false;

        WeakReferenceMessenger.Default.Send(new ChartLoadMessage(barChartWidth));
    }

    public Dictionary<string,float> Data { get; set; } = new Dictionary<string, float>();
}


public static class DateTimeExtensions
{
     public static string ToRFC3339(this DateTime date)
     {
         return date.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss");
     }
}
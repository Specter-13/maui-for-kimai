using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Core.Models;

public class TimesheetActiveModel
{

    public int Id { get; set; }
    public int ActivityId { get; set; }
    public string ActivityName { get; set; }

    public int ProjectId { get; set; }
    public string ProjectName { get; set; }

    public string CustomerName { get; set; }

    public double Duration { get; set; }




    //public static explicit operator TimesheetActiveModel(TimesheetEntityExpanded timesheet)
    //{ 
    //    return new TimesheetActiveModel
    //    { 
    //        Id = timesheet.Id.Value,
    //        ActivityId = timesheet.Activity.Id.Value,
    //        ActivityName = timesheet.Activity.Name,
    //        ProjectId = timesheet.Project.Id.Value,
    //        ProjectName = timesheet.Project.Name,
    //        CustomerName = timesheet.Project.Customer.Name,
    //        Duration = GetDuration(timesheet.Begin)
    //    };
    //}

    //public static explicit operator TimesheetActiveModel(TimesheetCollectionExpanded timesheet)
    //{ 
    //    return new TimesheetActiveModel
    //    { 
    //        Id = timesheet.Id.Value,
    //        ActivityId = timesheet.Activity.Id.Value,
    //        ActivityName = timesheet.Activity.Name,
    //        ProjectId = timesheet.Project.Id.Value,
    //        ProjectName = timesheet.Project.Name,
    //        CustomerName = timesheet.Project.Customer.Name,
    //        Duration = GetDuration(timesheet.Begin)
    //    };
    //}

    private static double GetDuration(DateTimeOffset begin)
    {
        var difference = DateTimeOffset.Now - begin;
        return difference.StripMilliseconds().TotalSeconds;
    }



}
public static class TimeExtensions
{
    public static TimeSpan StripMilliseconds(this TimeSpan time)
    {
        return new TimeSpan(time.Days, time.Hours, time.Minutes, time.Seconds);
    }
}
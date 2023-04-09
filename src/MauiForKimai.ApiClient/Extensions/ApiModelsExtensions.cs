using MauiForKimai.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ApiClient.Extensions;
public static class TimeExtensions
{
    public static TimeSpan StripMilliseconds(this TimeSpan time)
    {
        return new TimeSpan(time.Days, time.Hours, time.Minutes, time.Seconds);
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

public static class ApiModelsExtensions
{

    public static TimesheetActiveModel ToTimesheetActiveModel(this TimesheetCollectionExpanded timesheet)
    { 
        return new TimesheetActiveModel
        { 
            Id = timesheet.Id.Value,
            ActivityId = timesheet.Activity.Id.Value,
            ActivityName = timesheet.Activity.Name,
            ProjectId = timesheet.Project.Id.Value,
            ProjectName = timesheet.Project.Name,
            CustomerName = timesheet.Project.Customer.Name,
            Duration = GetDuration(timesheet.Begin)
        };
    }

    public static TimesheetActiveModel ToTimesheetActiveModel(this TimesheetEntityExpanded timesheet)
    {
        return new TimesheetActiveModel
        {
            Id = timesheet.Id.Value,
            ActivityId = timesheet.Activity.Id.Value,
            ActivityName = timesheet.Activity.Name,
            ProjectId = timesheet.Project.Id.Value,
            ProjectName = timesheet.Project.Name,
            CustomerName = timesheet.Project.Customer.Name,
            Duration = GetDuration(timesheet.Begin)
        };
    }

   

    public static TimesheetModel ToTimesheetModel(this TimesheetCollectionExpanded timesheet)
    { 
        return new TimesheetModel
        { 
            Id = timesheet.Id.Value,
           
            ActivityName = timesheet.Activity.Name,
            ProjectName = timesheet.Project.Name,
            CustomerName = timesheet.Project.Customer.Name,
            CustomerId = timesheet.Project.Customer.Id.Value,
            Date = timesheet.Begin.Date.ToShortDateString(),
            Duration = TimeSpan.FromSeconds(timesheet.Duration.Value).ToString(@"hh\:mm"),
            Begin = timesheet.Begin,
            End = timesheet.End,

            ActivityId= timesheet.Activity.Id.Value,
            ProjectId = timesheet.Project.Id.Value,
            Tags =  timesheet.Tags.Any() ? string.Join(",", timesheet.Tags) : null,
            Description = timesheet.Description,
            FixedRate = timesheet.Rate,
            Exported = timesheet.Exported,
            Billable = timesheet.Billable,
          
        };
    }

    public static TimesheetEditForm ToTimesheetEditFormFull(this TimesheetModel timesheet)
    { 
        return new TimesheetEditForm
        { 

            Begin = timesheet.Begin,
            End = timesheet.End,
            Project = timesheet.ProjectId,
            Activity = timesheet.ActivityId,
            Description = timesheet.Description,
            FixedRate = timesheet.FixedRate,
            HourlyRate = timesheet.HourlyRate,
            Exported = timesheet.Exported, 
            Billable = timesheet.Billable,
            Tags =  string.Join(",", timesheet.Tags),

        };
    }

    public static TimesheetEditForm ToTimesheetEditFormBase(this TimesheetModel timesheet)
    { 
        return new TimesheetEditForm
        { 

            Begin = timesheet.Begin,
            End = timesheet.End,
            Project = timesheet.ProjectId,
            Activity = timesheet.ActivityId,
            Description = timesheet.Description,
            Tags =  string.Join(",", timesheet.Tags),

        };
    }

 
    private static double GetDuration(DateTimeOffset begin)
    {
        var difference = DateTimeOffset.Now - begin;
        return difference.StripMilliseconds().TotalSeconds;
    }

}

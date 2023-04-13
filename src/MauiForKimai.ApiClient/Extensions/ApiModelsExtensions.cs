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

     public static TimeSpan StripSeconds(this TimeSpan time)
    {
        return new TimeSpan(time.Days, time.Hours, time.Minutes);
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

    public static DateTimeOffset ToDateTimeOffset(this DateTime dt, TimeSpan offset)
    {
        // adding negative offset to a min-datetime will throw, this is a 
        // sufficient catch. Note however that a DateTime of just a few hours can still throw
        if (dt == DateTime.MinValue)
            return DateTimeOffset.MinValue;

        return new DateTimeOffset(dt.Ticks, offset);
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
        int gitlabIssueId = 0;
        int.TryParse(timesheet.MetaFields?.FirstOrDefault(x => x.Name == "gitlab_issue_id")?.Value, out gitlabIssueId);
        return new TimesheetModel
        { 
            Id = timesheet.Id.Value,
           
            ActivityName = timesheet.Activity.Name,
            ProjectName = timesheet.Project.Name,
            CustomerName = timesheet.Project.Customer.Name,
            CustomerId = timesheet.Project.Customer.Id.Value,
            Date = timesheet.Begin.Date.ToShortDateString(),
            Duration = TimeSpan.FromSeconds(timesheet.Duration.Value).ToString(@"hh\:mm"),
            Begin = timesheet.Begin.DateTime,
            End = timesheet.End?.DateTime,

            ActivityId= timesheet.Activity.Id.Value,
            ProjectId = timesheet.Project.Id.Value,
            Tags =  timesheet.Tags.Any() ? string.Join(",", timesheet.Tags) : null,
            Description = timesheet.Description,
            FixedRate = timesheet.Rate,
            Exported = timesheet.Exported,
            Billable = timesheet.Billable,

            GitlabIssueId = gitlabIssueId == 0 ? null : gitlabIssueId,
          
        };
    }

    public static TimesheetEditForm ToTimesheetEditForm(this TimesheetModel timesheet, PermissionsTimetrackingModel permissions, TimeSpan offset)
    { 
        return new TimesheetEditForm
        { 

            Begin = timesheet.Begin.ToDateTimeOffset(offset),
            End = timesheet.End?.ToDateTimeOffset(offset),
            Project = timesheet.ProjectId,
            Activity = timesheet.ActivityId,
            Description = timesheet.Description,
            Tags =  string.Join(",", timesheet.Tags),


            FixedRate = permissions.CanEditRate ? timesheet.FixedRate : null,
            HourlyRate = permissions.CanEditRate ? timesheet.HourlyRate : null,
            Exported = permissions.CanEditExport ? timesheet.Exported : null,
            Billable = permissions.CanEditBillable? timesheet.Billable : null,
            

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

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

    public static TimesheetRecentListModel ToTimesheetRecentListModel(this TimesheetCollectionExpanded timesheet)
    { 
        return new TimesheetRecentListModel
        { 
            Id = timesheet.Id.Value,
            ActivityId = timesheet.Activity.Id.Value,
            ActivityName = timesheet.Activity.Name,
            ProjectId = timesheet.Project.Id.Value,
            ProjectName = timesheet.Project.Name,
            CustomerName = timesheet.Project.Customer.Name,
            Date = timesheet.Begin.Date.ToShortDateString(),
            Duration = TimeSpan.FromSeconds(timesheet.Duration.Value).ToString(@"hh\:mm")
        };
    }


    public static TimesheetListItemModel ToTimesheetListItemModel(this TimesheetCollectionExpanded timesheet)
    { 
        return new TimesheetListItemModel
        { 
            Id = timesheet.Id.Value,
           
            ActivityName = timesheet.Activity.Name,
            ProjectName = timesheet.Project.Name,
            CustomerName = timesheet.Project.Customer.Name,
            Date = timesheet.Begin.Date.ToShortDateString(),
            Duration = TimeSpan.FromSeconds(timesheet.Duration.Value).ToString(@"hh\:mm"),

            ActivityId= timesheet.Activity.Id.Value,
            ProjectId = timesheet.Project.Id.Value,
            Tags =  timesheet.Tags.Any() ? string.Join(",", timesheet.Tags) : null,
            Description = timesheet.Description,
            FixedRate = timesheet.Rate,
            Exported = timesheet.Exported,
            Billable = timesheet.Billable,
          
        };
    }

    public static TimesheetEditForm ToTimesheetEditFormRegularUser(this TimesheetListItemModel timesheet)
    { 
        return new TimesheetEditForm
        { 
            //From must be set manually to actual time
            
            Activity = timesheet.ActivityId,
            Project = timesheet.ProjectId,
            Tags =  string.Join(",", timesheet.Tags),
            Description = timesheet.Description,

        };
    }

 
    private static double GetDuration(DateTimeOffset begin)
    {
        var difference = DateTimeOffset.Now - begin;
        return difference.StripMilliseconds().TotalSeconds;
    }

}

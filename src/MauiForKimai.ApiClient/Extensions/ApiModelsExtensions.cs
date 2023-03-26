using MauiForKimai.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ApiClient.Extensions;
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

    private static double GetDuration(DateTimeOffset begin)
    {
        var difference = DateTimeOffset.Now - begin;
        return difference.StripMilliseconds().TotalSeconds;
    }

}

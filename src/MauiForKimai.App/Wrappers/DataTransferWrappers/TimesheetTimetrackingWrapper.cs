using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Wrappers;

public class TimesheetTimetrackingWrapper
{
    public TimesheetTimetrackingWrapper(TimesheetModel model, string activityName, string projectName, int gitlabIssueId)
    {
        Timesheet = model;
        ActivityName = activityName;
        ProjectName = projectName;
        GitlabIssueId = gitlabIssueId;
    }
    public TimesheetModel Timesheet { get; set; }
    public string ActivityName { get; set; }
    public string ProjectName{ get; set; }

    public int GitlabIssueId{ get; set; }
}

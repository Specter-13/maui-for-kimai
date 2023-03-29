using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Wrappers;

public class TimesheetTimetrackingWrapper
{
    public TimesheetTimetrackingWrapper(TimesheetEditForm form, string activityName, string projectName)
    {
        EditForm = form;
        ActivityName = activityName;
        ProjectName = projectName;
    }
    public TimesheetEditForm EditForm { get; set; }
    public string ActivityName { get; set; }
    public string ProjectName{ get; set; }
}

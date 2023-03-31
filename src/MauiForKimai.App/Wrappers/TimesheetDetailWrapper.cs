using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Wrappers;

public class TimesheetDetailWrapper
{
public TimesheetDetailWrapper(TimesheetListItemModel model, TimesheetDetailMode mode)
{
    Timesheet = model;
    Mode = mode;
}
public TimesheetListItemModel Timesheet { get; set; }
public TimesheetDetailMode Mode { get; set; }
}

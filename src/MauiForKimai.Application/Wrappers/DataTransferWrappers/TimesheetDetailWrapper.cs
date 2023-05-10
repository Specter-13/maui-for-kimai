using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Wrappers;

public class TimesheetDetailWrapper
{
public TimesheetDetailWrapper(TimesheetModel model, TimesheetDetailMode mode)
{
    Timesheet = model;
    Mode = mode;
}
public TimesheetModel Timesheet { get; set; }
public TimesheetDetailMode Mode { get; set; }
}

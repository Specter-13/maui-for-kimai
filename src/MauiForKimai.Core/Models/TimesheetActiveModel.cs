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

    public string ActivityColor { get; set; }
    public string ProjectColor { get; set; }

    public double Duration { get; set; }
    public DateTime Start { get; set; }

}

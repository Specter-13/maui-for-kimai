using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Core.Models;
public class TimesheetListItemModel
{
    public string? ActivityName { get; set; }
    public string? ProjectName { get; set; }
    public string? CustomerName { get; set; }

    public int? Duration { get; set; }
}

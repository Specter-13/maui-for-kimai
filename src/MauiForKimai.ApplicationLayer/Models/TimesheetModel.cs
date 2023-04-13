using MauiForKimai.Core.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Core.Models;


public class TimesheetModel
{
    public int Id { get; set; }

    //needed for list item
    public string ActivityName { get; set; }
    public string ProjectName { get; set; }
    public string? CustomerName { get; set; }
    public int? CustomerId { get; set; }
    public string Date { get; set; }
    public string Duration { get; set; }
    public bool IsRecent {get; set; }


    //needed for new timetracking
    public int ActivityId { get; set; }
    public int ProjectId { get; set; }
    public DateTime Begin { get; set; }

    public DateTime? End { get; set; } = null;
    public string? Tags { get; set; }= null;
    public string? Description { get; set; }= null;

    
    //needed for new higher role
    public float? FixedRate { get; set; } = null;
    public float? HourlyRate { get; set; } = null;
    public bool? Exported { get; set; } = null;
    public bool? Billable { get; set; } = null;

    public int? GitlabIssueId { get; set; }

}

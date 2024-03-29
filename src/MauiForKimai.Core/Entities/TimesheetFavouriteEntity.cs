﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Core.Entities;
public class TimesheetFavouriteEntity
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int ActivityId { get; set; }
    public string ActivityName { get; set; }
    public int ProjectId { get; set; }
    public string ProjectName { get; set; }
    public string? CustomerName { get; set; }
    public int? CustomerId { get; set; }
    public string? Description { get; set; }
    public string? Tags { get; set; }

    public bool? Billable { get; set; } 
    public bool? Exported { get; set; }
    public float? FixedRate { get; set; }
    public float? HourlyRate { get; set; }

    public int? GitlabIssueId { get; set; }

    public string ActivityColor { get; set; }
    public string ProjectColor { get; set; }
    public string CustomerColor { get; set; }
}

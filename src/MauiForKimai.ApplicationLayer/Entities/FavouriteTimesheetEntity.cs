﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Core.Entities;
public class FavouriteTimesheetEntity
{
    public int Id { get; set; }
    public int ActivityId { get; set; }
    public string ActivityName { get; set; }
    public int ProjectId { get; set; }
    public string ProjectName { get; set; }
    public string CustomerName { get; set; }


    public string? Billable { get; set; } 
    public bool? Exported { get; set; }
    public double? FixedRate { get; set; }
    public double? HourlyRate { get; set; }
}

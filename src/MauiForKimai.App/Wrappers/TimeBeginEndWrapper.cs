﻿using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using MauiForKimai.Core.Models;
using Microsoft.Maui.Controls;
using Newtonsoft.Json.Bson;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Wrappers;

public partial class TimeBeginEndWrapper : ObservableObject
{
    private readonly TimeSpan _offset;


    public TimeBeginEndWrapper(TimeSpan offset)
    {
        _offset = offset;
        var currentTime = DateTime.Now;
        BeginTime = currentTime.TimeOfDay;
        BeginDate = currentTime.Date;
        EndTime = currentTime.TimeOfDay;
        EndDate = currentTime.Date;
        BeginFull = new DateTime(BeginDate.Year, BeginDate.Month, BeginDate.Day, BeginTime.Hours, BeginTime.Minutes, BeginTime.Seconds);
        EndFull = BeginFull;
        BeginFullString = BeginFull.ToString("dddd, dd MMMM yyyy");
        //Duration = "00:00";
    }

    public TimeBeginEndWrapper(TimesheetModel timesheet,TimeSpan offset)
    {
        _offset = offset;
       
        var beginTime = timesheet.Begin.TimeOfDay;
        var beginDate = timesheet.Begin.Date;

        BeginFull = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day, beginTime.Hours, beginTime.Minutes, beginTime.Seconds);
        BeginDate = BeginFull.Date;
        BeginTime = BeginFull.TimeOfDay;

        if(timesheet.End.HasValue)
        { 
            var endTime = timesheet.End.Value.TimeOfDay;
            var endDate = timesheet.End.Value.Date;
            EndFull = new DateTime(endDate.Year, endDate.Month, endDate.Day, endTime.Hours, endTime.Minutes, endTime.Seconds);
            EndDate = EndFull.Value.Date;
            EndTime = EndFull.Value.TimeOfDay;
        }

      
         //SetDuration();
        BeginFullString = BeginFull.ToString("dddd, dd MMMM yyyy");
    }

    [ObservableProperty]
    string beginFullString;

    [ObservableProperty]
    bool isNotDurationFormatValid;



    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Duration))]
    public DateTime beginDate, endDate;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Duration))]
    public TimeSpan beginTime, endTime;

 
    public string Duration => GetDuration(); 


    public void UpdateEnd(TimeSpan duration)
    {
        //EndFull = new DateTimeOffset(EndDate.Year, EndDate.Month, EndDate.Day, EndTime.Hours, EndTime.Minutes, EndTime.Seconds, _offset);
        //BeginFull = new DateTimeOffset(BeginDate.Year, BeginDate.Month, BeginDate.Day, BeginTime.Hours, BeginTime.Minutes, BeginTime.Seconds, _offset);
        var newEnd = BeginFull.Add(duration);
        EndTime = newEnd.TimeOfDay;
        EndDate = newEnd.Date;
    }
   

    private string GetDuration()
    {

        if(EndFull == null)
            return $"00:00";

        EndFull = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, EndTime.Hours, EndTime.Minutes, EndTime.Seconds);
        BeginFull = new DateTime(BeginDate.Year, BeginDate.Month, BeginDate.Day, BeginTime.Hours, BeginTime.Minutes, BeginTime.Seconds);
        var difference = (EndFull.Value - BeginFull);
        var hours = ((int)difference.TotalHours).ToString("00");
        var minutes = difference.Minutes.ToString("00");
        return $"{hours}:{minutes}";
    
        
    }


    public DateTime BeginFull { get; set; }
    public DateTime? EndFull { get; set; }
}

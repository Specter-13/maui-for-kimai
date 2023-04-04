﻿using MauiForKimai.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Core;
public static class CoreModelExtensions
{

    //TimesheetFavouriteEntity
    public static TimesheetModel ToTimesheetModel(this TimesheetFavouriteEntity entity)
    {
        return new TimesheetModel
        {
            Id = entity.Id,
            ActivityId = entity.ActivityId,
            ActivityName = entity.ActivityName,
            ProjectId = entity.ProjectId,
            ProjectName = entity.ProjectName,
            CustomerName = entity.CustomerName,

            Description = entity.Description,
            Tags = entity.Tags,
         
            Billable = entity.Billable,
            FixedRate = entity.FixedRate,
            Exported = entity.Exported,
            HourlyRate = entity.HourlyRate
        };
    }

    //TimesheetModel
    public static TimesheetFavouriteEntity ToTimesheetFavouriteEntity(this TimesheetModel model)
    {
        return new TimesheetFavouriteEntity
        {
            Id = model.Id,
            ActivityId = model.ActivityId,
            ActivityName = model.ActivityName,
            ProjectId = model.ProjectId,
            ProjectName = model.ProjectName,
            CustomerName = model.CustomerName,

            Description = model.Description,
            Tags = model.Tags,
         
            Billable = model.Billable,
            FixedRate = model.FixedRate,
            Exported = model.Exported,
            HourlyRate = model.HourlyRate
        };
    }

     //ServerEntity
    public static ServerModel ToServerModel (this ServerEntity entity)
    {
        return new ServerModel
        {
            Id = entity.Id,
            Url = entity.Url,
            Name = entity.Name,
            Username = entity.Username,
            IsDefault = entity.IsDefault,

        };
    }
    //ServerModel
   public static ServerEntity ToServerEntity(this ServerModel entity)
    {
        return new ServerEntity
        {
            Id = entity.Id,
            Url = entity.Url,
            Name = entity.Name,
            Username = entity.Username,
            IsDefault = entity.IsDefault,

        };
    }


}

using MauiForKimai.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Core.Models;
public class TimesheetFavouritesListModel
{

    public int Id { get; set; }
    public string Name { get; set; }
    public string ActivityName { get; set; }
    public string ProjectName { get; set; }


    public static explicit operator TimesheetFavouritesListModel(TimesheetFavouriteEntity entity)
    {
        return new TimesheetFavouritesListModel
        {
            Id = entity.Id,
            Name = entity.Name,
            ActivityName = entity.ActivityName,
            ProjectName = entity.ProjectName,
        };
    }
}

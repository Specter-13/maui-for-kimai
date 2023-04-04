using MauiForKimai.Core.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Core.Models;


public class TimesheetListItemGroupModel : List<TimesheetListItemModel>
{
    public string Name { get; private set; }




    public TimesheetListItemGroupModel(string name, List<TimesheetListItemModel> timesheets) : base(timesheets)
    {
        Name = name;
    }
}

public class TimesheetListItemModel
{
    public int Id { get; set; }

    //needed for list item
    public string ActivityName { get; set; }
    public string ProjectName { get; set; }
    public string CustomerName { get; set; }
    public int CustomerId { get; set; }
    public string Date { get; set; }
    public string Duration { get; set; }



    //needed for new timetracking
    public int ActivityId { get; set; }
    public int ProjectId { get; set; }
    public DateTimeOffset Begin { get; set; }

    public DateTimeOffset? End { get; set; }
    public string Tags { get; set; }
    public string Description { get; set; }
    public float? FixedRate { get; set; }
    public bool? Exported { get; set; }
    public bool? Billable { get; set; }

      public static explicit operator TimesheetListItemModel(TimesheetFavouriteEntity entity)
    {
        return new TimesheetListItemModel
        {
            Id = entity.Id,
            ActivityId = entity.ActivityId,
            ActivityName = entity.ActivityName,
            ProjectId = entity.ProjectId,
            ProjectName = entity.ProjectName,
            CustomerName = entity.CustomerName,

            Tags = entity.Tags,
            Description = entity.Description,
            FixedRate = entity.FixedRate,
            Exported = entity.Exported,
            Billable = entity.Billable
        };
    }

}

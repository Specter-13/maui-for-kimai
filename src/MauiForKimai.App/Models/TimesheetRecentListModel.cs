using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Models
{
    public class TimesheetRecentListModel
    {
        public int Id { get; set; }
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string CustomerName { get; set; }
        public string Date { get; set; }
     
        public string Duration { get; set; }


        public static explicit operator TimesheetRecentListModel(TimesheetCollectionExpanded timesheet)
        { 
            return new TimesheetRecentListModel
            { 
                Id = timesheet.Id.Value,
                ActivityId = timesheet.Activity.Id.Value,
                ActivityName = timesheet.Activity.Name,
                ProjectId = timesheet.Project.Id.Value,
                ProjectName = timesheet.Project.Name,
                CustomerName = timesheet.Project.Customer.Name,
                Date = timesheet.Begin.Date.ToShortDateString(),
                Duration = TimeSpan.FromSeconds(timesheet.Duration.Value).ToString(@"hh\:mm")
            };
        }

    }

 
}

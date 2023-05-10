using MauiForKimai.ApiClient.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Wrappers;

public partial class StatisticsWrapper
{

    public StatisticsWrapper()
    {
    }

    

    public string TodayTracked { get; set; }

    public string MostTrackedProjectName { get; set; }
    public string MostTrackedProjectDuration { get; set; }

    public string NumberOfActivites { get; set; }
    public string NumberOfProjects{ get; set; }

    public Task CalculateTodayStatistics(ICollection<TimesheetCollectionExpanded> todayTimesheets)
    { 
        activityTimes = new();
        return Task.Run( () => {

		    int todayDuration = 0;
            int numberOfactivities = 0;
            KeyValuePair<string,int> longestActivity = new KeyValuePair<string, int>("",0);
		    foreach (var timesheet in todayTimesheets)
		    {
                var name = timesheet.Activity.Name;
                if(timesheet.Duration == null) continue;

                var duration = timesheet.Duration.Value;

                if(activityTimes.ContainsKey(name)) 
                {
                    activityTimes[name] += duration;
                }
                else
                {
                    activityTimes.Add(name, duration);
                    numberOfactivities++;
                }

			    todayDuration += duration;
		    }

            TodayTracked = TimeSpan.FromSeconds(todayDuration).ToString(@"hh\:mm") + " h";

            if(numberOfactivities == 1)
            {
                NumberOfActivites = $"{numberOfactivities} activity";
            }
            else
            {
                NumberOfActivites = $"{numberOfactivities} activities";
            }
            var numberOfProjects = todayTimesheets.Select(x=>x.Project.Name).Distinct().Count();
            if(numberOfProjects == 1)
            {
                NumberOfProjects = $"{numberOfProjects} project";
            }
            else
            {
                NumberOfProjects = $"{numberOfProjects} projects";
            }
            
        });
	}

    private Dictionary<string, int> activityTimes = new Dictionary<string, int>();
}

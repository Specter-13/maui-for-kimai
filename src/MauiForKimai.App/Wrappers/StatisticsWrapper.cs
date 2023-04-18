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

    public string LongestActivityName { get; set; }
    public string LongestActivityDuration { get; set; }

    public string LongestActivityProjectName { get; set; }

    public string NumberOfActivites { get; set; }
    public string NumberOfProjects{ get; set; }

    public Task CalculateTodayStatistics(ICollection<TimesheetCollectionExpanded> todayTimesheets)
    { 
        activityTimes = new();
        return Task.Run( () => {

		    int todayDuration = 0;
            int numberOfactivities = 0;
            string longestActivityProjectName = "";
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

                if (activityTimes[name] > longestActivity.Value) 
                {
                    longestActivity = new KeyValuePair<string, int>(name,activityTimes[name]);
                    longestActivityProjectName = timesheet.Project.Name;
                }

			    todayDuration += duration;
		    }

            TodayTracked = TimeSpan.FromSeconds(todayDuration).ToString(@"hh\:mm") + " h";
            LongestActivityDuration = TimeSpan.FromSeconds(longestActivity.Value).ToString(@"hh\:mm") + " h";
            LongestActivityName = longestActivity.Key;
            LongestActivityProjectName = longestActivityProjectName;

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

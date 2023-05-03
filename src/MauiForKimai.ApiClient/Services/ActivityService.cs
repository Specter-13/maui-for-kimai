using MauiForKimai.ApiClient.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ApiClient.Services;
public class ActivityService : BaseService, IActivityService
{
    public ActivityService(ApiClientWrapper aw) : base(aw)
    {
    }

    public Task<ICollection<ActivityCollection>> GetActivities()
    {
       return base._aw.ApiClient?.ActivitiesAllAsync(null,null,null,null,null,null,null,null);
    }

    public Task<ICollection<ActivityCollection>> GetActivitiesByProject(int projectId)
    {
       return base._aw.ApiClient?.ActivitiesAllAsync(projectId.ToString(),null,null,null,null,"project","DESC",null);
    }

      public Task<ICollection<ActivityCollection>> GetGlobalActivities()
    {
       return base._aw.ApiClient?.ActivitiesAllAsync(null,null,null,"true",null,null,null,null);
    }
}   

using MauiForKimai.ApiClient.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ApiClient.Services;
public class ProjectService : BaseService, IProjectService
{
    public ProjectService(ApiClientWrapper aw) : base(aw)
    {
    }


    public Task<ICollection<ProjectCollection>> GetProjects()
    {
        return _aw.ApiClient?.ProjectsAllAsync(null,null,null,null,null,null,null,null,null,null);
    }


    public Task<ICollection<ProjectCollection>> GetProjectsByCustomer(int customerId)
    {
        return _aw.ApiClient?.ProjectsAllAsync(customerId.ToString(),null,null,null,null,null,null,null,null,null);
    }
    public Task<ProjectEntity> Create(ProjectEditForm entity)
    {
        return _aw.ApiClient?.ProjectsPOSTAsync(entity);
    }

    public Task DeleteProjectFromTeam(int teamId, int projectId)
    {
         return _aw.ApiClient?.ProjectsDELETEAsync(teamId, projectId);
    }

    public Task<ProjectEntity> Read(int id)
    {
        return _aw.ApiClient?.ProjectsGETAsync(id.ToString());
    }

    public Task<ProjectEntity> Update(int id, ProjectEditForm body)
    {
        return _aw.ApiClient?.ProjectsPATCHAsync(body, id);
    }
}

using MauiForKimai.ApiClient.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ApiClient.Services;
public class ProjectService : BaseService, IProjectService
{
    public ProjectService(IHttpClientFactory httpClientFactory, ApiStateProvider asp) : base(httpClientFactory, asp)
    {
    }


     public Task<ICollection<ProjectCollection>> GetProjects()
    {
        return ApiClient.ProjectsAllAsync(null,null,null,null,null,null,null,null,null,null);
    }

    public Task<ProjectEntity> Create(ProjectEditForm entity)
    {
        return ApiClient.ProjectsPOSTAsync(entity);
    }

    public Task DeleteProjectFromTeam(int teamId, int projectId)
    {
         return ApiClient.ProjectsDELETEAsync(teamId, projectId);
    }

    public Task<ProjectEntity> Read(int id)
    {
        return ApiClient.ProjectsGETAsync(id.ToString());
    }

    public Task<ProjectEntity> Update(int id, ProjectEditForm body)
    {
        return ApiClient.ProjectsPATCHAsync(body, id);
    }
}

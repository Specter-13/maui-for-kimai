using MauiForKimai.ApiClient.Authentication;
using MauiForKimai.ApiClient;
using MauiForKimai.ApiClient.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ApiClient.Services;
public class TimesheetService : BaseService, ITimesheetService
{
    //private ITimesheetClient _timesheetClient;
    public TimesheetService(IHttpClientFactory httpClientFactory, ApiStateProvider asp) : base(httpClientFactory,asp)
    {
    }

    public Task<TimesheetEntity> Create(TimesheetEditForm entity)
    {
        if(entity == null)
            return null;

        
         return ApiClient?.TimesheetsPOSTAsync(entity,null);
       
        
    }

    public Task Delete(int id)
    {
        return  ApiClient?.TimesheetsDELETEAsync(id);
    }


    public Task<ICollection<TimesheetCollectionExpanded>> GetTimesheetsIncrementalyAsync(int page, int sizePerPage)
	{ 
        
        return  ApiClient?.TimesheetsAllExpandedAsync(ApiStateProvider.ActualUser.Id.ToString(),null,null,null,null,null,null,page.ToString(),sizePerPage.ToString(),null,null,null,null,null,null,null,null,"true",null,null);
	}


    public Task<ICollection<TimesheetCollectionExpanded>> GetTimesheetsForReportsAsync(string begin, string end)
	{ 
        //dd-mm-yyyy
        return  ApiClient?.TimesheetsAllExpandedAsync(null,null,null,null,null,null,null,null,null,null,null,null,begin,end,null,null,null,"true",null,null);;
	}
    public Task<ICollection<TimesheetCollectionExpanded>> GetTenRecentTimesheetsAsync()
    {
        //if(ApiClient == null) return null;
            return  ApiClient?.RecentAsync(base.ApiStateProvider.ActualUser.Id.ToString(),null,null);
    }

    public Task<TimesheetEntity> Read(int id)
    {
        return  ApiClient?.TimesheetsGETAsync(id);
    }

    public Task<TimesheetEntity> Update(int id, TimesheetEditForm body)
    {
        return  ApiClient?.TimesheetsPATCHAsync(id, body);
    }
    public Task<ICollection<TimesheetCollectionExpanded>> GetActive()
    {
        return  ApiClient?.ActiveAsync();
    }
    public Task<TimesheetEntity> StopActive(int id)
    {
        return  ApiClient?.StopAsync(id);
    }
}

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

    public Task<ICollection<TimesheetCollection>> GetAllTimesheetsAsync()
	{ 
        return null;
        //return ApiClient.TimesheetsAllAsync();
		//return _timesheetClient.TimesheetsAllAsync();
		//return _timesheetClient.Get_get_timesheetsAsync();
	}

    public Task<ICollection<TimesheetCollectionExpanded>> GetTenRecentTimesheetsAsync()
    {
        return ApiClient.RecentAsync(base.ApiStateProvider.ActualUser.Id.ToString(),null,null);
        //return  _timesheetClient.Get_recent_timesheetAsync(base.ApiStateProvider.ActualUser.Id.ToString());
    }

 //   public void InitializeClient(string baseUrl)
	//{
	//	base.CreateNewHttpClient(baseUrl);
	//	//_timesheetClient = new TimesheetClient(base._httpClient);
	//}
}

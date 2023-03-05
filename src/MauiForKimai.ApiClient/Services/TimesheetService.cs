using MauiForKimai.ApiClient.ApiClient;
using MauiForKimai.ApiClient.Authentication;
using MauiForKimai.ApiClient.Client;
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
    private ITimesheetClient _timesheetClient;
    public TimesheetService(IHttpClientFactory httpClientFactory, ApiStateProvider asp) : base(httpClientFactory,asp)
    {
    }

    public Task<ICollection<TimesheetCollection>> GetAllTimesheetsAsync()
	{ 
		return _timesheetClient.TimesheetsAllAsync();
		//return _timesheetClient.Get_get_timesheetsAsync();
	}

    public Task<ICollection<TimesheetCollectionExpanded>> GetTenRecentTimesheetsAsync()
    {
        return _timesheetClient.RecentAsync(base.ApiStateProvider.ActualUser.Id.ToString());
        //return  _timesheetClient.Get_recent_timesheetAsync(base.ApiStateProvider.ActualUser.Id.ToString());
    }

    public void InitializeClient(string baseUrl)
	{
		base.CreateNewHttpClient(baseUrl);
		_timesheetClient = new TimesheetClient(base._httpClient);
	}
}

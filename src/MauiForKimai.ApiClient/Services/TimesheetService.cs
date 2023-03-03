using MauiForKimai.ApiClient.Authentication;
using MauiForKimai.ApiClient.Client;
using MauiForKimai.ApiClient.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ApiClient.Services;
public class TimesheetService : BaseService, ITimesheetService
{
    private ITimesheetClient _timesheetClient;
    public TimesheetService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
    }

    public Task<ICollection<TimesheetCollection>> GetAllTimesheetsAsync()
	{ 
		
		return _timesheetClient.TimesheetsAllAsync();
	}

    public void InitializeClient(string baseUrl)
	{
		_httpClient.BaseAddress = new Uri(baseUrl);
		 _timesheetClient = new TimesheetClient(_httpClient);
	}
}

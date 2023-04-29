﻿using MauiForKimai.ApiClient.Authentication;
using MauiForKimai.ApiClient;
using MauiForKimai.ApiClient.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MauiForKimai.ApiClient.Extensions;

namespace MauiForKimai.ApiClient.Services;
public class TimesheetService : BaseService, ITimesheetService
{
    //private ITimesheetClient _timesheetClient;
    public TimesheetService(IHttpClientFactory httpClientFactory, ApiLoginContext asp) : base(httpClientFactory,asp)
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
        
        return  ApiClient?.TimesheetsAllExpandedAsync(base.loginContext.ActualUser.Id.ToString(),null,null,null,null,null,null,page.ToString(),sizePerPage.ToString(),null,null,null,null,null,null,null,null,"true",null,null);
	}


    public Task<ICollection<TimesheetCollectionExpanded>> GetTimesheetsForReportsAsync(string begin, string end)
	{ 
        //dd-mm-yyyy
        return  ApiClient?.TimesheetsAllExpandedAsync(null,null,null,null,null,null,null,null,null,null,null,null,begin,end,null,null,null,"true",null,null);;
	}

     public Task<ICollection<TimesheetCollectionExpanded>> GetTodayTimesheetsAsync()
	{ 
        //dd-mm-yyyy
        var today = DateTime.Now;
        var begin = new DateTime(today.Year,today.Month,today.Day);
        var end = new DateTime(today.Year,today.Month,today.Day,23,59,59);

        try
        {
            return  ApiClient?.TimesheetsAllExpandedAsync(null,null,null,null,null,null,null,null,null,null,null,null,begin.ToRFC3339(),end.ToRFC3339(),null,null,null,"true",null,null);;
        }
        catch (KimaiApiException)
        {
            return null;
        }
	}
    public Task<ICollection<TimesheetCollectionExpanded>> GetTenRecentTimesheetsAsync()
    {
        try
        {
            return  ApiClient?.RecentAsync(base.loginContext.ActualUser.Id.ToString(),null,null);
        }
        catch (KimaiApiException)
        {

            return null;
        }
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
        try
        {
            return  ApiClient?.ActiveAsync();
        }
        catch (KimaiApiException)
        {
            return null;
        }
    }
    public Task<TimesheetEntity> StopActive(int id)
    {
        try
        {
            return  ApiClient?.StopAsync(id);
        }
        catch (KimaiApiException)
        {
            return null;
        }
    }

    public Task<TimesheetEntity> SetMetaField(int id, Body5 body)
    {   
        return  ApiClient?.Meta4Async(id,body);
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using MauiForKimai.ApiClient;
using MauiForKimai.ApiClient.Authentication;
using MauiForKimai.ApiClient.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ApiClient.Services;
public class BaseService : IBaseService
{
	protected IHttpClientFactory _httpClientFactory;
	protected HttpClient? _httpClient;
	protected ApiLoginContext loginContext;
    public BaseService(IHttpClientFactory httpClientFactory, ApiLoginContext asp) 
	{
		_httpClientFactory = httpClientFactory;
		loginContext = asp;
	}

	public IApiClient? ApiClient {get; set;}

	public bool IsClientInitialized() => ApiClient != null;

    public void InitializeClient(string baseUrl)
	{ 
		_httpClient = _httpClientFactory.CreateClient(AuthHandler.AUTHENTICATED_CLIENT);
		_httpClient.BaseAddress = new Uri(baseUrl);
		ApiClient = new ApiClient(_httpClient);
	}

	public void DeInitializeClient()
	{ 
		if(_httpClient != null)
		{
			_httpClient.Dispose();
		}

		if(ApiClient != null) 
		{
			ApiClient = null;
		}
	
		
	}
	public async Task<bool> PingServerAsync()
	{
		try
		{
			await ApiClient?.PingAsync();
			return true;
		}
		catch 
		{
			return false;
		}
		

	}

	public async Task<I18nConfig> GetI18nConfig()
	{
		return await ApiClient?.I18nAsync();
	}


	
}

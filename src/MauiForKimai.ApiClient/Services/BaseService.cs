using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using MauiForKimai.ApiClient.ApiClient;
using MauiForKimai.ApiClient.Authentication;
using MauiForKimai.ApiClient.Client;
using MauiForKimai.ApiClient.Interfaces;
using MauiForKimai.ApplicationLayer.Messages;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ApiClient.Services;
public class BaseService 
{
	protected IHttpClientFactory _httpClientFactory;
	protected HttpClient _httpClient;
	protected ApiStateProvider ApiStateProvider;

    public BaseService(IHttpClientFactory httpClientFactory, ApiStateProvider asp) 
	{
		_httpClientFactory = httpClientFactory;
		ApiStateProvider = asp;
	}

    protected void CreateNewHttpClient(string baseUrl)
	{ 
		_httpClient = _httpClientFactory.CreateClient(AuthHandler.AUTHENTICATED_CLIENT);
		_httpClient.BaseAddress = new Uri(baseUrl);
	}
	public async Task<bool> PingServerAsync()
	{
		var defualtClient = new DefaultClient(_httpClient);
		try
		{
			await defualtClient.PingAsync();
			return true;
		}
		catch 
		{
			return false;
		}
		

	}


	
}

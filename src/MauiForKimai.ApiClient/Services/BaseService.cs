using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
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
	protected HttpClient _httpClient;

	public BaseService(IHttpClientFactory httpClientFactory) 
	{
		_httpClient = httpClientFactory.CreateClient(AuthHandler.AUTHENTICATED_CLIENT);
	}

	
	


	
}

using MauiForKimai.ApiClient.Authentication;
using MauiForKimai.ApiClient.Client;
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
	protected AuthHandler _auth;
	public BaseService(IHttpClientFactory httpClientFactory, AuthHandler auth) 
	{
		_httpClient = httpClientFactory.CreateClient(AuthHandler.AUTHENTICATED_CLIENT);
		_auth = auth;
	}

	
}

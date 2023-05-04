using MauiForKimai.ApiClient.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ApiClient;

public class ApiClientWrapper
{
   
	private IHttpClientFactory _httpClientFactory;
	private HttpClient? _httpClient;
	public ApiLoginContext loginContext {get;}

    public ApiClientWrapper(IHttpClientFactory httpClientFactory, ApiLoginContext asp) 
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
		if(ApiClient != null) 
		{
			ApiClient = null;
		}
	}

	public async Task<I18nConfig> GetI18nConfig()
	{
		return await ApiClient?.I18nAsync();
	}
}

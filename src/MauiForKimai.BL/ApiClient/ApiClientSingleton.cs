using MauiForKimai.BL.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.BL.ApiClient;

public class ApiClientSingleton
{
    public static ApiClientSingleton? ApiClient;

    private static readonly object _lock = new object();
    private ApiClientSingleton(HttpClient client) 
    {
        UserClient = new UserClient(client);
        DefaultClient = new DefaultClient(client);
    }


    public UserClient UserClient {get; private set;}
    public DefaultClient DefaultClient {get; private set;}
    public static ApiClientSingleton CreateInstance(HttpClient client)
    {
        if (ApiClient == null)
        {
            lock(_lock)
            {
                if (ApiClient == null)
                    ApiClient = new ApiClientSingleton(client);
            }
        }
        return ApiClient;
    }

    public static ApiClientSingleton GetExistingInstance()
    {
        if (ApiClient == null)
        {
            throw new NonExistingApiClientException("ApiClient instance does not exist!");
        }
        return ApiClient;
    }
}


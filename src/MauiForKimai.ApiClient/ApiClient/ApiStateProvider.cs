using CommunityToolkit.Mvvm.ComponentModel;
using MauiForKimai.ApiClient.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ApiClient.ApiClient;

public partial class ApiStateProvider : ObservableObject
{
    public string UserName {get; private set; } = string.Empty;
    public string ApiPassword {get; private set; } = string.Empty;
    public string BaseUrl {get; private set; } = string.Empty;

    public UserEntity ActualUser {get; set;}

    [ObservableProperty]
    public bool isAuthenticated;

    public void SetAuthInfo(string token, string apiPassword, string baseUrl)
    {
        UserName = token;
        ApiPassword = apiPassword;
        BaseUrl = baseUrl;
    }
    public void SetIsAuthenticated()
    {
        IsAuthenticated = true;
    }
    public void Disconnect()
    {
        IsAuthenticated = false;
        UserName= string.Empty;
        ApiPassword= string.Empty;
        BaseUrl= string.Empty;
        ActualUser = null;
    }

}

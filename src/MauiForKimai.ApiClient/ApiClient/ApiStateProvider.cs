using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ApiClient;

public partial class ApiStateProvider : ObservableObject
{
    public string UserName {get; private set; } = string.Empty;
    public string ApiPassword {get; private set; } = string.Empty;
    public string BaseUrl {get; private set; } = string.Empty;

    public UserEntity? ActualUser {get; private set;}


    [ObservableProperty]
    public bool isTeamlead;

    [ObservableProperty]
    public bool isAdmin;

    [ObservableProperty]
    public bool isSuperAdmin;

    [ObservableProperty]
    public bool isAuthenticated;

    public void SetAuthInfo(string token, string apiPassword, string baseUrl)
    {
        UserName = token;
        ApiPassword = apiPassword;
        BaseUrl = baseUrl;
    }

    public void SetLoggedUser(UserEntity user)
    {
        ActualUser = user;
    }
    private const string ROLE_ADMIN = "ROLE_ADMIN";
    private const string ROLE_SUPER_ADMIN = "ROLE_SUPER_ADMIN";
    private const string ROLE_TEAMLEAD = "ROLE_TEAMLEAD";

    public void SetRoles(UserEntity user)
    {

        foreach (var role in user.Roles)
        {
            if(role == ROLE_SUPER_ADMIN )
            { 
                IsTeamlead = true;
                IsAdmin = true;
                IsSuperAdmin = true;
                break;
            }

            if(role == ROLE_ADMIN)
            { 
                IsTeamlead = true;
                IsAdmin = true;
            }

            if(role == ROLE_TEAMLEAD  )
            { 
                IsTeamlead = true;
            }
        }
        
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
        IsAdmin = false;
        IsSuperAdmin = false;
        IsTeamlead = false;
    }

}

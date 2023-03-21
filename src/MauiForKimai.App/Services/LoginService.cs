using MauiForKimai.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Services;

public class LoginService : ILoginService
{

    private readonly IEnumerable<IBaseService> _baseServices;
    private readonly IUserService _userService;
     private readonly ApiStateProvider _asp;
    private List<ServerModel> Servers {get; set; } = new();
    public LoginService(ISecureStorageService secureStorageService,
        IEnumerable<IBaseService> baseServices, 
        IUserService userService,
        ApiStateProvider asp) 
    {

         var local = new ServerModel()
        {
            Id= 1,
            Username = "admin@admin.com",
            ApiPasswordKey = "internet",
            IsDefault = true,
            Name = "My local server",
            Url = "http://localhost:8001/"
            
        };

        _asp = asp;
        _baseServices = baseServices;
        _userService = userService;
        Servers.Add(local);
        
    }
    public async Task<bool> LoginToDefaultOnStartUp()
    {
       
        var defaultServer = Servers.FirstOrDefault(x=> x.IsDefault == true);
        if (defaultServer == null)
             return false;

        _asp.SetAuthInfo(defaultServer.Username,defaultServer.ApiPasswordKey,defaultServer.Url);  
        InitializeClients(defaultServer.Url);

         _asp.ActualUser = await _userService.GetMe();

        if(_asp.ActualUser == null )
        { 
            // connection failed
            return false;
        }

        //connection successfull
        _asp.SetIsAuthenticated();
         return true;

    }

    Task ILoginService.Login()
    {
        throw new NotImplementedException();
    }

    Task ILoginService.Logout()
    {
        throw new NotImplementedException();
    }

    Task ILoginService.TryToGetDefaultServer()
    {
        throw new NotImplementedException();
    }

     private void InitializeClients(string baseUrl)
    {
        foreach (var baseService in _baseServices)
        {
            baseService.InitializeClient(baseUrl);
        }
    }
}

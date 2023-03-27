using MauiForKimai.ApiClient;

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

    public LoginService(IEnumerable<IBaseService> baseServices, 
        IUserService userService,
        ApiStateProvider asp) 
    {

       //  var local = new ServerModel()
       // {
       //     Id= 1,
       //     Username = "admin@admin.com",
       //     ApiPasswordKey = "internet",
       //     IsDefault = true,
       //     Name = "My local server",
       //     Url = "http://localhost:8001/"
            
       // };

       //var demo = new ServerModel()
       // {
       //     Id= 2,
       //     Username = "john_user",
       //     ApiPasswordKey = "kitten",
       //     IsDefault = false,
       //     Name = "Demo server online",
       //     Url = "https://demo-plugins.kimai.org/"
            
       // };

       //  var localJan = new ServerModel()
       // {
       //     Id= 2,
       //     Username = "jan@jan.com",
       //     ApiPasswordKey = "internet",
       //     IsDefault = false,
       //     Name = "My local server Jan",
       //     Url = "http://localhost:8001/"
            
       // };
        

        _asp = asp;
        _baseServices = baseServices;
        _userService = userService;
        
    }
    public ApiStateProvider GetApiStateProvider()
    {
        return _asp;
    }

    public async Task<bool> TestConnection(ServerModel server)
    {
        bool isSuccess;
        try
        {
            var oldUserName = _asp.UserName;
            var oldApiPsw = _asp.ApiPassword;
            var oldUrl = _asp.BaseUrl;

             DeInitializeClients();
            _asp.SetAuthInfo(server.Username,server.ApiPasswordKey,server.Url);  
            InitializeClients(server.Url);
            await _userService.PingServerAsync();

            //is success
            //TODO - fix unlogin when testing connection!

            isSuccess = true;
        }
        catch (Exception)
        {
            isSuccess = false;
        }

        _asp.Disconnect();
        DeInitializeClients();
        return isSuccess;
    }

    public bool IsLogged()
    {
        return _asp.IsAuthenticated;
    }

    public async Task<bool> LoginToDefaultOnStartUp(ServerModel defaultServer)
    {
        if (defaultServer == null)
             return false;
        
        return await TryToLogin(defaultServer);
    }

    public bool CheckIfConnected(ServerModel server)
    {
        if(_asp.IsAuthenticated && _asp.BaseUrl == server.Url && 
           _asp.UserName == server.Username && _userService.IsClientInitialized()) 
        {
            return true;
        }

        return false;

    }

    public Task<bool> Login(ServerModel server)
    {
        return TryToLogin(server); 

    }

    public Task Logout()
    {
        return Task.Run(() =>
        {
            _asp.Disconnect();
            DeInitializeClients();
        });
       
    }

    public TimeSpan GetUserTimeOffset() => _userTimeOffset;


    private TimeSpan _userTimeOffset;

    private async Task<bool> TryToLogin(ServerModel server)
    { 
        try
        {
            _asp.Disconnect();
            _asp.SetAuthInfo(server.Username,server.ApiPasswordKey,server.Url);  
            InitializeClients(server.Url);
            var user = await _userService.GetMe();
            _asp.SetLoggedUser(user);
            _asp.SetRoles(user);
            //set timezone offset by server
            var config = await _userService.GetI18nConfig();
            _userTimeOffset = config.Now.Value.Offset;
        }
        catch (Exception e)
        {
            _asp.Disconnect();
            DeInitializeClients();
            return false;
        }
        _asp.SetIsAuthenticated();
        return true;
    }


  
    private void InitializeClients(string baseUrl)
    {
        foreach (var baseService in _baseServices)
        {
            baseService.InitializeClient(baseUrl);
        }
    }

    private void DeInitializeClients()
    {
        foreach (var baseService in _baseServices)
        {
            baseService.DeInitializeClient();
        }
    }


}

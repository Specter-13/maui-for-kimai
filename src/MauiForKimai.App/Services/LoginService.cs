using MauiForKimai.ApiClient;
using MauiForKimai.Persistence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Services;

public class LoginService : ILoginService
{

    private readonly IUserService _userService;
    private ApiClientWrapper _aw { get ;set; }

    public LoginService(IUserService userService, ApiClientWrapper aw) 
    {
        _userService = userService;
        _aw = aw;
    }
    public ApiLoginContext GetLoginContext()
    {
        return _aw.loginContext;
    }


    public async Task<bool> LoginOnStartUp(ServerModel defaultServer)
    {
        if (defaultServer == null)
             return false;
        
        return await LoginToKimai(defaultServer);
    }

    public bool CheckIfConnected(ServerModel server)
    {
        if(_aw.loginContext.IsAuthenticated && _aw.loginContext.BaseUrl == server.Url && 
           _aw.loginContext.UserName == server.Username && _aw.IsClientInitialized()) 
        {
            return true;
        }

        return false;

    }

    public Task<bool> Login(ServerModel server)
    {
        return LoginToKimai(server); 
    }


    public Task Logout()
    {
        return Task.Run(() =>
        {
            _aw.loginContext.Disconnect();
            DeInitializeApiClient();
            Task.Delay(1000);
        });
       
    }


    private async Task<bool> LoginToKimai(ServerModel server)
    { 
        try
        {
            _aw.loginContext.Disconnect();
            _aw.loginContext.SetAuthInfo(server); 
            InitializeApiClient(server.Url);
            var config = await _aw.GetI18nConfig();
            var user = await _userService.GetMe();
            _aw.loginContext.SetUserAndOffset(user,config.Now.Value.Offset);
            _aw.loginContext.SetIsAuthenticated();

        }
        catch (Exception)
        {
            _aw.loginContext.Disconnect();
            DeInitializeApiClient();
            await Task.Delay(1000);
            return false;
        }
        
        return true;
    }

    private void InitializeApiClient(string baseUrl)
    {
        _aw.InitializeClient(baseUrl);
    }

    private void DeInitializeApiClient()
    {
        _aw.DeInitializeClient();
    }
  

}

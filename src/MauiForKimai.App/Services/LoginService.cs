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

    private readonly IEnumerable<IBaseService> _baseServices;
    private readonly IUserService _userService;
    private readonly ApiLoginContext _loginContext;

    public LoginService(IEnumerable<IBaseService> baseServices, 
        IUserService userService,
        ApiLoginContext asp, IServerService serverService) 
    {

        _loginContext = asp;
        _baseServices = baseServices;
        _userService = userService;
    }
    public ApiLoginContext GetLoginContext()
    {
        return _loginContext;
    }


    public async Task<bool> LoginOnStartUp(ServerModel defaultServer)
    {
        if (defaultServer == null)
             return false;
        
        return await TryToLogin(defaultServer);
    }

    public bool CheckIfConnected(ServerModel server)
    {
        if(_loginContext.IsAuthenticated && _loginContext.BaseUrl == server.Url && 
           _loginContext.UserName == server.Username && _userService.IsClientInitialized()) 
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
            _loginContext.Disconnect();
            DeInitializeClients();
        });
       
    }


    private async Task<bool> TryToLogin(ServerModel server)
    { 
        try
        {
            _loginContext.Disconnect();
            _loginContext.SetAuthInfo(server); 
            InitializeClients(server.Url);
            var config = await _userService.GetI18nConfig();
            var user = await _userService.GetMe();
            _loginContext.SetUserAndOffset(user,config.Now.Value.Offset);
            _loginContext.SetIsAuthenticated();

        }
        catch (Exception)
        {
            _loginContext.Disconnect();
            DeInitializeClients();
            return false;
        }
        
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

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
    private readonly IServerService _serverService;
    private readonly ApiStateProvider _asp;

    public LoginService(IEnumerable<IBaseService> baseServices, 
        IUserService userService,
        ApiStateProvider asp, IServerService serverService) 
    {

        _asp = asp;
        _baseServices = baseServices;
        _userService = userService;
        _serverService = serverService;
    }
    public ApiStateProvider GetApiStateProvider()
    {
        return _asp;
    }



    public async Task<bool> LoginOnStartUp(ServerModel defaultServer)
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
            //TODO send message to refresh data
        });
       
    }

    public TimeSpan GetUserTimeOffset() => _userTimeOffset;


    private TimeSpan _userTimeOffset;

    private async Task<bool> TryToLogin(ServerModel server)
    { 
        try
        {
            _asp.Disconnect();

            _asp.SetAuthInfo(server);  
            InitializeClients(server.Url);
            var user = await _userService.GetMe();
            _asp.SetUser(user);
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

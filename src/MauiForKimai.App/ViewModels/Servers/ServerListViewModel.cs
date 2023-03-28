using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MauiForKimai.ApiClient.Authentication;
using MauiForKimai.ApiClient.Interfaces;
using MauiForKimai.ApiClient.Services;
using MauiForKimai.Messenger;
using MauiForKimai.ViewModels.Base;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ViewModels;
public partial class ServerListViewModel : ViewModelBase
{
    
 

    private readonly IServerService _serverService;
    private readonly ISecureStorageService  _secureStorageService;

    public ServerListViewModel(IRoutingService rs, 
        ILoginService ls, 
        IServerService serverService,
        ISecureStorageService sc) : base(rs, ls)
    {
        _serverService = serverService;
        _secureStorageService = sc;
        //TODO use secure storage for api password token
        //var local = new ServerModel()
        //{
        //    Id= 1,
        //    Username = "admin@admin.com",
        //    ApiPasswordKey = "internet",
        //    IsDefault = true,
        //    Name = "My local server",
        //    Url = "http://localhost:8001/"
            
        //};

        //var demo = new ServerModel()
        //{
        //    Id= 2,
        //    Username = "john_user",
        //    ApiPasswordKey = "kitten123",
        //    IsDefault = false,
        //    Name = "Demo server online",
        //    Url = "https://demo-plugins.kimai.org/"
            
        //};

        //var localJan = new ServerModel()
        //{
        //    Id= 2,
        //    Username = "jan@jan.com",
        //    ApiPasswordKey = "internet",
        //    IsDefault = false,
        //    Name = "My local server Jan",
        //    Url = "http://localhost:8001/"
            
        //};
        
        WeakReferenceMessenger.Default.Register<ServerAcquireMessage>(this, async (r, m) =>
        {
			await GetServersFromDb();
        });
        
    }


    public override async Task Initialize()
    {
        await GetServersFromDb();
    }

    private async Task GetServersFromDb()
    { 
        IsBusy = true;
        var servers = await _serverService.GetAll();
        Servers.Clear();
        foreach (var server in servers) 
        {
            if(server.IsDefault)
            {
                Servers.Insert(0,server);
            }
            else
            { 
                Servers.Add(server);
            }
        }
        IsBusy = false;
    }
    public ObservableCollection<ServerEntity> Servers {get; set; } = new();


    [RelayCommand]
    async Task ServerTapped(ServerEntity server) 
    {
        var route = routingService.GetRouteByViewModel<ServerDetailViewModel>();
        await Navigation.NavigateTo(route, server);
    }

    [RelayCommand]
    async Task AddNewServer() 
    {

        var route = routingService.GetRouteByViewModel<ServerDetailViewModel>();
        await Navigation.NavigateTo(route, true);


    }
    
}

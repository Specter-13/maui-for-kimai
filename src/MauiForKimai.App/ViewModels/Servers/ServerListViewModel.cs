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

    public ServerListViewModel(IRoutingService rs, 
        ILoginService ls, 
        IServerService serverService) : base(rs, ls)
    {
        _serverService = serverService;
       
        
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

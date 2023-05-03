using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LiveChartsCore;
using MauiForKimai.ApiClient.Authentication;
using MauiForKimai.ApiClient.Interfaces;
using MauiForKimai.ApiClient.Services;
using MauiForKimai.Core.Models;
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
public partial class ServerListViewModel : ViewModelBase, IViewModelSingleton
{

    private readonly IServerService _serverService;
    private readonly IFavouritesTimesheetService _favouritesTimesheetService;
    private readonly ISecureStorageService  _secureStorageService;
    public ServerListViewModel(IRoutingService rs, 
        ILoginService ls, 
        IFavouritesTimesheetService fts,
        IServerService serverService,
        ISecureStorageService sc) : base(rs, ls)
    {
        _serverService = serverService;
       _favouritesTimesheetService = fts;
        _secureStorageService = sc;
        
        WeakReferenceMessenger.Default.Register<ServerRefreshMessage>(this, async (r, m) =>
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


    [RelayCommand]
    async Task QuickConnect(ServerEntity server) 
    {
        if(base.GetConnectivity() == NetworkAccess.Internet
            )
        { 
            IsBusy = true;
            if (loginService.CheckIfConnected(server.ToServerModel()))
            {
                var toast = Toast.Make($"Already connected to {server.Name}!", ToastDuration.Short, 14);
                await toast.Show();
                return;
            }

            var key = $"{server.Id}_{server.Username}";
            var serverModel = server.ToServerModel();
            try
            {
            
                var apiPassword = await _secureStorageService.Get(key);
            
                serverModel.ApiPasswordKey = apiPassword;
            }
            catch (KeyNotFoundException)
            {
                var toast = Toast.Make("Api key was not found in secure storage! Delete server and try to create it again.", ToastDuration.Short, 14);
                await toast.Show();
            }


            var isSuccess = await loginService.Login(serverModel);
            if(isSuccess) 
            {
                //reinit db
                await _favouritesTimesheetService.ReInit();
            
                await Toast.Make("Connection to Kimai established!", ToastDuration.Short, 14).Show();
                OnPropertyChanged(nameof(LoginContext));
            }
		    else
		    {
			    await Toast.Make("Connection to Kimai failed! Check your credentials!", ToastDuration.Short, 14).Show();
                OnPropertyChanged(nameof(LoginContext));
		    }
            IsBusy = false;
            //wait a second to be sure, that everything is deinitialized
            await Task.Delay(1000);
            WeakReferenceMessenger.Default.Send(new RefreshMessage(string.Empty));
        }
        else
        { 
            await Toast.Make("Check your internet connection", ToastDuration.Short, 14).Show();
        }
    }
    
}

using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MauiForKimai.ApiClient.Authentication;
using MauiForKimai.ApiClient.Interfaces;
using MauiForKimai.ApiClient.Services;
using MauiForKimai.ApplicationLayer.Messages;
using MauiForKimai.Models;
using MauiForKimai.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ViewModels;
public partial class ServerListViewModel : ViewModelBase
{
    
    public ObservableCollection<ServerModel> Servers {get; set; } = new();
    private readonly IEnumerable<IBaseService> _baseServices;
    private readonly IUserService _userService;
    public ServerListViewModel(IRoutingService rs, ILoginService ls, IEnumerable<IBaseService> baseServices, IUserService userService) : base(rs, ls)
    {

        //TODO use secure storage for api password token
        var local = new ServerModel()
        {
            Id= 1,
            Username = "admin@admin.com",
            ApiPasswordKey = "internet",
            IsDefault = true,
            Name = "My local server",
            Url = "http://localhost:8001/"
            
        };

        var demo = new ServerModel()
        {
            Id= 2,
            Username = "john_user",
            ApiPasswordKey = "kitten123",
            IsDefault = false,
            Name = "Demo server online",
            Url = "https://demo-plugins.kimai.org/"
            
        };

        var localJan = new ServerModel()
        {
            Id= 2,
            Username = "jan@jan.com",
            ApiPasswordKey = "internet",
            IsDefault = false,
            Name = "My local server Jan",
            Url = "http://localhost:8001/"
            
        };
        

   
        _baseServices = baseServices;
        _userService = userService;
        Servers.Add(local);
        Servers.Add(demo);
         Servers.Add(localJan);

    }

    [RelayCommand]
    async Task ServerTapped(ServerModel server) 
    {
        var route = routingService.GetRouteByViewModel<ServerDetailViewModel>();
        await Navigation.NavigateTo(route, server);
    }

    [RelayCommand]
    async Task AddNewServer() 
    {
        //await Navigation.NavigateTo(nameof(ServerDetailPage));
    }

    [RelayCommand]
    async Task Logout() 
    {
          base.ApiStateProvider.Disconnect();
        //await Navigation.NavigateTo(nameof(ServerDetailPage));
    }

    

    private void InitializeClients(string baseUrl)
    {
        foreach (var baseService in _baseServices)
        {
            baseService.InitializeClient(baseUrl);
        }
    }
}

using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MauiForKimai.ApiClient.Authentication;
using MauiForKimai.ApiClient.Interfaces;
using MauiForKimai.ApiClient.Services;
using MauiForKimai.ApplicationLayer.Messages;
using MauiForKimai.Models;
using MauiForKimai.Pages.ServersManagement;
using MauiForKimai.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ViewModels;
public partial class LoginViewModel : ViewModelBase
{
    
    public ObservableCollection<ServerModel> Servers {get; set; } = new();
    private readonly IEnumerable<IBaseService> _baseServices;
    private readonly IUserService _userService;
    public LoginViewModel(ApiStateProvider asp, IEnumerable<IBaseService> baseServices, IUserService userService, IRoutingService routingService) : base(asp, routingService)
    {

        //TODO use secure storage for api password token
        var local = new ServerModel()
        {
            Id= 1,
            Username = "admin@admin.com",
            ApiPasswordKey = "internet",
            IsDefault = false,
            Name = "My local server",
            Url = "http://localhost:8001/"
            
        };

   
        _baseServices = baseServices;
        _userService = userService;
        Servers.Add(local);

    }



    [RelayCommand]
    async Task AddNewServer() 
    {
        await Shell.Current.GoToAsync(nameof(ServerDetailPage));
    }

    [RelayCommand]
    async Task Connect(ServerModel server) 
    {

		base.ApiStateProvider.SetAuthInfo(server.Username,server.ApiPasswordKey,server.Url);  
        InitializeClients(server.Url);
        var isConnected = await _baseServices.First().PingServerAsync();


        ToastDuration duration = ToastDuration.Short;
		double fontSize = 14;
		if(isConnected) 
        { 
            base.ApiStateProvider.SetIsAuthenticated();
            //var id = await _userService.GetUserByIdAsync(1);
            //var allUsers = await _userService.GetAllUsersAsync();
            base.ApiStateProvider.ActualUser = await _userService.GetMe();

            string text = "Connection to Kimai established!";
		    

		    var toast = Toast.Make(text, duration, fontSize);
		    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
		    await toast.Show(cancellationTokenSource.Token);

        }
        else
        { 
            string text = "Connection failed! Check your credentials!";
		    var toast = Toast.Make(text, duration, fontSize);
		    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
		    await toast.Show(cancellationTokenSource.Token);
        }
    }

    private void InitializeClients(string baseUrl)
    {
        foreach (var baseService in _baseServices)
        {
            baseService.InitializeClient(baseUrl);
        }
    }
}

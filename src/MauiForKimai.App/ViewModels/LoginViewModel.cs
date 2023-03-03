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
    private readonly AuthHandler _authHandler;
    private readonly IEnumerable<IBaseService> _baseServices;
    public LoginViewModel(AuthHandler aut, IEnumerable<IBaseService> baseServices) : base(aut)
    {
        _authHandler = aut;
        var defaultServer = new ServerModel()
        {
            Id= 0,
            Username = "dadkos34@gmail.com",
            ApiPasswordKey = "internet",
            IsDefault = true,
            Name = "My Kimai server",
            Url = "https://specter13maui.kimai.cloud/"
            
        };
        _baseServices = baseServices;
        Servers.Add(defaultServer);

    }



    [RelayCommand]
    async Task AddNewServer() 
    {
        await Shell.Current.GoToAsync(nameof(ServerDetailPage));
    }

    [RelayCommand]
    async Task Connect(ServerModel server) 
    {

		_authHandler.SetAuthInfo(server.Url,server.Username,server.ApiPasswordKey);
        
        InitializeClients(server.Url);
		_authHandler.SetIsAuthenticated();
       
    }

    private void InitializeClients(string baseUrl)
    {
        foreach (var baseService in _baseServices)
        {
            baseService.InitializeClient(baseUrl);
        }
    }
}

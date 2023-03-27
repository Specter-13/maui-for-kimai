using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MauiForKimai.ApiClient.Authentication;
using MauiForKimai.Messenger;
using MauiForKimai.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ViewModels;
public partial class ServerDetailViewModel : ViewModelBase
{

	[ObservableProperty]
	private string apiPassword;

    [ObservableProperty]
	private bool isCreation;

    [ObservableProperty]
	private bool isLoggedToThisServer;

    
    [ObservableProperty]
	private bool isConnecting;

    private readonly IServerService _serverService;
    private readonly ISecureStorageService  _secureStorageService;
    [ObservableProperty]
    private ServerModel server = new();

    public ServerDetailViewModel(IRoutingService rs, ILoginService ls, IServerService ss, ISecureStorageService sc) : base(rs, ls)
    {
        _serverService = ss;
        _secureStorageService = sc;
    }


    public override async Task OnParameterSet()
    {
        IsBusy = true;

        if (NavigationParameter is ServerEntity myServer)
        {
            Server = (ServerModel) myServer;
            IsLoggedToThisServer = loginService.CheckIfConnected(Server);
            var key = $"{Server.Id}_{Server.Username}";

            try
            {
                  var apiPassword = await _secureStorageService.Get(key);
                  Server.ApiPasswordKey = apiPassword;
            }
            catch (KeyNotFoundException)
            {
                var toast = Toast.Make("Api key was not found in secure storage! Delete server and try to create it again.", ToastDuration.Short, 14);
                await toast.Show();
            }
        }

        if (NavigationParameter is bool creation)
        {
            if(creation == true)
            { 
                IsCreation = creation;
            }

        }

        IsBusy = false;
    }

    
    [RelayCommand]
    async Task ConnectandCreate() 
    {
       IsConnecting = true;
       var isSuccess = await loginService.Login(Server);
        
       IToast toast;
       if(!isSuccess) 
       {
            toast = Toast.Make("Connection to Kimai failed! Check your credentials!", ToastDuration.Short, 14);
            await toast.Show();
            return;
       }

        //create server
        var newServer = await _serverService.Create(Server);

        //create api key in secure storage
        var key = $"{newServer.Id}_{newServer.Username}";
        await _secureStorageService.Save(key, Server.ApiPasswordKey);
        
        toast = Toast.Make("Connection to Kimai established!", ToastDuration.Short, 14);
        IsCreation = false;
        IsLoggedToThisServer = true;
        WeakReferenceMessenger.Default.Send(new ServerAcquireMessage(string.Empty));
        await toast.Show();
        IsConnecting = false;
    }


    [RelayCommand]
    async Task Connect() 
    {
        IsConnecting = true;
        if (loginService.CheckIfConnected(Server))
        {
            var toast = Toast.Make($"Already connected to {Server.Name}!", ToastDuration.Short, 14);
            await toast.Show();
            return;
        }

        await ConnectToServer();
        IsConnecting = false;
    }

    private async Task ConnectToServer()
    { 
   
        var isSuccess = await loginService.Login(Server);
        IToast toast;
        if(isSuccess) 
        {
            toast = Toast.Make("Connection to Kimai established!", ToastDuration.Short, 14);
        }
		else
		{
			toast = Toast.Make("Connection to Kimai failed! Check your credentials!", ToastDuration.Short, 14);
		}
        await toast.Show();
    }

    [RelayCommand]
    async Task TestConnection() 
    {

		var isSuccess = await loginService.TestConnection(Server);
        IToast toast;
        if(isSuccess) 
        {
            toast = Toast.Make("Server is reachable! Credentials are correct.", ToastDuration.Short, 14);
        }
		else
		{
			toast = Toast.Make("Server cannot be reached! Check your credentials and internet connection.", ToastDuration.Short, 14);
		}
        await toast.Show();
    }

    [RelayCommand]
    async Task Disconnect() 
    {

		await loginService.Logout();
        await Toast.Make("Disconected successfully!", ToastDuration.Short, 14).Show();

    }

   [RelayCommand]
    async Task Delete() 
    {
        var isLogged = loginService.CheckIfConnected(Server);
        if(isLogged)
        { 
		    await loginService.Logout();
        }
        //remove secure key if exists
        var key = $"{Server.Id}_{Server.Username}";
        _secureStorageService.Remove(key);

        //delete server from db
        await _serverService.Delete(Server.Id);

        //notify and navigate back
        WeakReferenceMessenger.Default.Send(new ServerAcquireMessage(string.Empty));
        await Navigation.NavigateTo("..");

    }
}

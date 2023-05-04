using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Maui.Views;
using MauiForKimai.ApiClient.Authentication;
using MauiForKimai.Messenger;
using MauiForKimai.Popups;
using MauiForKimai.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiForKimai.Core;
using MauiForKimai.Core.Validators;
using MauiForKimai.ApiClient;

namespace MauiForKimai.ViewModels;
public partial class ServerDetailViewModel : ViewModelBase, IViewModelTransient
{


    [ObservableProperty]
	private bool isCreation;


    [ObservableProperty]
	private bool isLoggedToThisServer = false;
    
    [ObservableProperty]
	private bool isConnecting;


    [ObservableProperty]
	private bool hasConnectionButton;

    [ObservableProperty]
    public bool showErrors;

    [ObservableProperty]
    public bool overrideTimetrackingPermissions;

    [ObservableProperty]
    public string validationErrors;

    private ServerModelValidator _validator = new ();

    private readonly IServerService _serverService;
    private readonly IFavouritesTimesheetService _favouritesTimesheetService;
    private readonly ISecureStorageService  _secureStorageService;

    private bool previousIsDefaultValue;
    private string previousUserName;

    [ObservableProperty]
    private ServerModel server = new();

    public ServerDetailViewModel(IRoutingService rs, 
        ILoginService ls, 
        IServerService ss, 
        ISecureStorageService sc,
        IFavouritesTimesheetService fts) : base(rs, ls)
    {
        _serverService = ss;
        _favouritesTimesheetService = fts;
        _secureStorageService = sc;
    }



   

    private async Task ReinitializeDatabases()
    {
        await _favouritesTimesheetService.ReInit();
    }
    public override async Task OnParameterSet()
    {
        IsBusy = true;

        if (NavigationParameter is ServerEntity myServer)
        {
            Server = myServer.ToServerModel();
            previousIsDefaultValue = myServer.IsDefault;
            previousUserName = myServer.Username;
            IsLoggedToThisServer = loginService.CheckIfConnected(Server);
            var key = $"{Server.Id}_{Server.Username}";

            try
            {
                  var apiPassword = await _secureStorageService.Get(key);
                  Server.ApiPasswordKey = apiPassword;
                  OnPropertyChanged(nameof(Server));
            }
            catch (KeyNotFoundException)
            {
                var toast = Toast.Make("Api key was not found in secure storage! Delete server and try to create it again.", ToastDuration.Short, 14);
                await toast.Show();
            }
            if(!IsLoggedToThisServer) HasConnectionButton = true;
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
        ValidationErrors = string.Empty;
        var result = _validator.Validate(Server);
        if (result.IsValid)
        {
           IsConnecting = true;
           if(loginService.CheckIfConnected(Server))
           {
                await loginService.Logout();
                IsLoggedToThisServer = false;
           }
           Server.Id = Guid.NewGuid();
           var isSuccess = await loginService.Login(Server);
        

           if(!isSuccess) 
           {
                await Toast.Make("Connection to Kimai failed! Check your credentials!", ToastDuration.Short, 14).Show();
                IsLoggedToThisServer = false;
                 IsConnecting = false;
                return;
           }

            //set timesheet permissions automatically if ovverride is off
           if(!OverrideTimetrackingPermissions)
               SetPermissionsByRoles(base.LoginContext.ActualUser.Roles);

            await UnsetDefaultServerIfChanged();


            //create server
            var newServer = await _serverService.Create(Server);

            //create api key in secure storage
            var key = $"{newServer.Id}_{newServer.Username}";
            await _secureStorageService.Save(key, Server.ApiPasswordKey);
        

            IsCreation = false;
            IsLoggedToThisServer = true;
            await ReinitializeDatabases();

            WeakReferenceMessenger.Default.Send(new RefreshMessage(string.Empty));
            WeakReferenceMessenger.Default.Send(new ServerRefreshMessage(string.Empty));
            await Toast.Make("Connection to Kimai established!", ToastDuration.Short, 14).Show();
            OnPropertyChanged(nameof(LoginContext));
            IsConnecting = false;
            HasConnectionButton = IsCreation && !IsLoggedToThisServer;
        }
        else
        { 
            ValidationErrors = result.ToString("\n");
            await Toast.Make("Form validation failed.", ToastDuration.Short, 14).Show();
            ShowErrors = true;
        }
        
    }

    private void SetPermissionsByRoles(ICollection<string> roles)
    {

        foreach (var role in roles)
        {
            if (role == UserRole.ROLE_SUPER_ADMIN || role == UserRole.ROLE_ADMIN)
            {
               
                Server.CanEditBillable = true;
                Server.CanEditExport = true;
                Server.CanEditRate = true;
                LoginContext.TimetrackingPermissions.CanEditBillable = true;
                LoginContext.TimetrackingPermissions.CanEditExport = true;
                LoginContext.TimetrackingPermissions.CanEditRate = true;
                break;
            }

            if (role == UserRole.ROLE_TEAMLEAD)
            {
                Server.CanEditBillable = true;
                Server.CanEditExport = true;
                LoginContext.TimetrackingPermissions.CanEditBillable = true;
                LoginContext.TimetrackingPermissions.CanEditExport = true;
            }

        }

        OnPropertyChanged(nameof(Server));
    }


    [RelayCommand]
    async Task Connect() 
    {
        ValidationErrors = string.Empty;
        var result = _validator.Validate(Server);
        if (result.IsValid)
        {
            IsConnecting = true;
            if (loginService.CheckIfConnected(Server))
            {
                var toast = Toast.Make($"Already connected to {Server.Name}!", ToastDuration.Short, 14);
                await toast.Show();
                IsConnecting = false;
                return;
            }

            await ConnectToServer();
            //wait a second to be sure, that everything is deinitialized
            await Task.Delay(1000);
            WeakReferenceMessenger.Default.Send(new RefreshMessage(string.Empty));
            IsConnecting = false;
            ValidationErrors = string.Empty;
        }
        else
        {
            ValidationErrors = result.ToString("\n");
            IsLoggedToThisServer = false;
            await Toast.Make("Form validation failed.", ToastDuration.Short, 14).Show();
        }
    }

    private async Task<bool> ConnectToServer()
    {
    
        if(base.GetConnectivity() == NetworkAccess.Internet)
        { 
            var isSuccess = await loginService.Login(Server);
       
            if(isSuccess) 
            {
        
                await ReinitializeDatabases();
                OnPropertyChanged(nameof(LoginContext));
                await Toast.Make("Connection to Kimai established!", ToastDuration.Short, 14).Show();
                IsLoggedToThisServer = true;
                HasConnectionButton = false;
                return true;
            }
		    else
		    {
			    await Toast.Make("Connection to Kimai failed! Check your credentials!", ToastDuration.Short, 14).Show();
                IsLoggedToThisServer = false;
                HasConnectionButton = true;
                return false;
		    }
        }
        else
        { 
            await Toast.Make("Check your internet connection!", ToastDuration.Short, 14).Show();
            return false; 
        }
       
    }


    [RelayCommand]
    async Task Disconnect() 
    {
        IsBusy = true;
        ValidationErrors = string.Empty;
        //wait a second to be sure, that everything is deinitialized
        await Task.Delay(2500);
		await loginService.Logout();

        OnPropertyChanged(nameof(LoginContext));
       
        WeakReferenceMessenger.Default.Send(new RefreshMessage(string.Empty));
        WeakReferenceMessenger.Default.Send(new FavouritesRefreshMessage(string.Empty));
        IsLoggedToThisServer = false;
        HasConnectionButton = true;
        await Toast.Make("Disconected successfully!", ToastDuration.Short, 14).Show();
        await Navigation.NavigateTo("..");
        IsBusy = false;
    }


    [RelayCommand]
    async Task Save() 
    {
        ValidationErrors = string.Empty;
        var result = _validator.Validate(Server);
        if (result.IsValid)
        {
            IsBusy = true;

            await UnsetDefaultServerIfChanged();
        
            
            //if i'm logged to this server, logout and try to login again with new credentialss
            if(IsLoggedToThisServer)
            {
                await loginService.Logout();
                var isSuccess = await loginService.Login(Server);
                if(!isSuccess)
                { 
                    IsConnecting = false;
                    IsBusy = false;
                    await Toast.Make("Connection to Kimai failed! Check your credentials!", ToastDuration.Short, 14).Show();
                    return;
                }
                else
                {
                    await Toast.Make("Reconnect successfull.", ToastDuration.Short, 14).Show();
                }
            }

            //delete previous key from secure storage
            var key = $"{Server.Id}_{previousUserName}";
            _secureStorageService.Remove(key);

            //create new key
            key = $"{Server.Id}_{Server.Username}";
            await _secureStorageService.Save(key, Server.ApiPasswordKey);

            //update changes in db
            await _serverService.Update(Server);

            await Toast.Make("Save successfull!", ToastDuration.Short, 14).Show();
            WeakReferenceMessenger.Default.Send(new ServerRefreshMessage(string.Empty));
            IsBusy = false;
            ValidationErrors = string.Empty;
        }
        else
        { 
            ValidationErrors = result.ToString("\n");
            await Toast.Make("Form validation failed.", ToastDuration.Short, 14).Show();
            IsLoggedToThisServer = false;
        }
    }

   

   [RelayCommand]
    async Task Delete() 
    {
        IsBusy = true;
        var isLogged = loginService.CheckIfConnected(Server);
        if(isLogged)
        { 
            //wait a second to be sure, that everything is deinitialized
            await Task.Delay(2500);
            _favouritesTimesheetService.DeleteDatabase($"maui_for_kimai_server_db_{base.LoginContext.ServerId}");
		    await loginService.Logout();
            WeakReferenceMessenger.Default.Send(new RefreshMessage(string.Empty));
        }
        //remove secure key if exists
        var key = $"{Server.Id}_{Server.Username}";
        _secureStorageService.Remove(key);

        //delete server from db
        await _serverService.Delete(Server.Id);

        //notify and navigate back
        IsLoggedToThisServer = false;
        await ReinitializeDatabases();
        WeakReferenceMessenger.Default.Send(new ServerRefreshMessage(string.Empty));
        await Navigation.NavigateTo("..");
        IsBusy = false;
    }

    
    private async Task UnsetDefaultServerIfChanged()
    { 
        //if there was a change of default server, ask user for consent to override default server
        if(previousIsDefaultValue != Server.IsDefault && Server.IsDefault == true) 
        {
            bool answer = await Page.DisplayAlert("Override default server", "Are you sure, that you want to override default server?", "Yes", "No");
            if(!answer)
            { 
                return;
            }
            await _serverService.UnsetDefaultPropertyExceptOne(Server.Id);
        }
    }
   
}

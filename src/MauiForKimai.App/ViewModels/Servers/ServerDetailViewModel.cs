﻿using CommunityToolkit.Maui.Alerts;
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
    private readonly IFavouritesTimesheetService _favouritesTimesheetService;
    private readonly ISecureStorageService  _secureStorageService;
    private readonly PopupSizeConstants _popupSizeConstants;

    private bool previousIsDefaultValue;
    private string previousUserName;

    [ObservableProperty]
    private ServerModel server = new();

    public ServerDetailViewModel(IRoutingService rs, 
        ILoginService ls, 
        IServerService ss, 
        ISecureStorageService sc,
        PopupSizeConstants psc,
        IFavouritesTimesheetService fts) : base(rs, ls)
    {
        _serverService = ss;
        _favouritesTimesheetService = fts;
        _secureStorageService = sc;
        _popupSizeConstants = psc;
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

    private bool Validate()
    {
        return !string.IsNullOrEmpty(Server.Name) && !string.IsNullOrEmpty(Server.Url) &&
            !string.IsNullOrEmpty(Server.Username) && !string.IsNullOrEmpty(Server.ApiPasswordKey);
    }

    [ObservableProperty]
    public bool overrideTimetrackingPermissions;

    [RelayCommand]
    async Task ConnectandCreate() 
    {
       IsConnecting = true;

       var isSuccess = await loginService.Login(Server);
        
       IToast toast;
       if(!isSuccess) 
       {
            await Toast.Make("Connection to Kimai failed! Check your credentials!", ToastDuration.Short, 14).Show();
            return;
       }

        await UnsetDefaultServerIfChanged();


        //set timesheet permissions automatically if ovverride is off
        if(!OverrideTimetrackingPermissions)
            SetPermissionsByRoles(base.LoginContext.ActualUser.Roles);
   

        //server.PermissionsTimetracking.CanSetBillable

        //create server
        var newServer = await _serverService.Create(Server);

        //create api key in secure storage
        var key = $"{newServer.Id}_{newServer.Username}";
        await _secureStorageService.Save(key, Server.ApiPasswordKey);
        
        toast = Toast.Make("Connection to Kimai established!", ToastDuration.Short, 14);
        IsCreation = false;
        IsLoggedToThisServer = true;
        await ReinitializeDatabases();
        WeakReferenceMessenger.Default.Send(new ServerAcquireMessage(string.Empty));
        await toast.Show();
        IsConnecting = false;
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
                break;
            }

            if (role == UserRole.ROLE_TEAMLEAD)
            {
                Server.CanEditBillable = true;
                Server.CanEditExport = true;
            }

        }

        OnPropertyChanged(nameof(Server));
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

    private async Task<bool> ConnectToServer()
    { 
        var isValid = Validate();
        if (!isValid) 
        {
            await Toast.Make("Form validation failed!", ToastDuration.Short, 14).Show();
            return false;
        }
        var isSuccess = await loginService.Login(Server);
        if(isSuccess) 
        {
            await ReinitializeDatabases();
            await Toast.Make("Connection to Kimai established!", ToastDuration.Short, 14).Show();
            return true;
        }
		else
		{
			await Toast.Make("Connection to Kimai failed! Check your credentials!", ToastDuration.Short, 14).Show();
            return false;
		}
    }


    [RelayCommand]
    async Task Disconnect() 
    {

		await loginService.Logout();
        await Toast.Make("Disconected successfully!", ToastDuration.Short, 14).Show();

    }


    [RelayCommand]
    async Task Save() 
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
                await Toast.Make("Connection to Kimai failed! Check your credentials!", ToastDuration.Short, 14).Show();
                return;
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

        await Toast.Make("Connection and save successfull!", ToastDuration.Short, 14).Show();
        WeakReferenceMessenger.Default.Send(new ServerAcquireMessage(string.Empty));
        IsBusy = false;

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

    
    private async Task UnsetDefaultServerIfChanged()
    { 
        //if there was a change of default server, ask user for consent to override default server
        if(previousIsDefaultValue != Server.IsDefault && Server.IsDefault == true) 
        {
            var isDefaultOverride = await DisplayServerDefaultPopup();
            if(!isDefaultOverride)
            { 
                return;
            }
            await _serverService.UnsetDefaultPropertyExceptOne(Server.Id);
        }
    }
    private async Task<bool> DisplayServerDefaultPopup()
    {
        var popup = new ServerDefaultPopup(_popupSizeConstants);

        return (bool) await Page.ShowPopupAsync(popup);
    }
}

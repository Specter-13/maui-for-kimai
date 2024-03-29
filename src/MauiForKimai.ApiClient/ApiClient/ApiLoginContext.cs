﻿using CommunityToolkit.Mvvm.ComponentModel;
using MauiForKimai.Core.Entities;
using MauiForKimai.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ApiClient;

public partial class ApiLoginContext : ObservableObject
{

    internal string ApiPassword {get; private set; } = string.Empty;


    [ObservableProperty]
    public string userName = string.Empty;
    [ObservableProperty]
    public string baseUrl = string.Empty;

    [ObservableProperty]
    public string serverFirstLetter = string.Empty;

    public Guid ServerId {get; private set; }


    public UserEntity? ActualUser {get; private set;}

  
    public TimeSpan TimeOffset {get; set;}

    [ObservableProperty]
    public bool isAuthenticated;

    [ObservableProperty]
    public string? serverName;

    [ObservableProperty]
    public PermissionsTimetrackingModel? timetrackingPermissions;

    public void SetAuthInfo(ServerModel server)
    {
        UserName = server.Username;
        ApiPassword = server.ApiPasswordKey;
        BaseUrl = server.Url;
        ServerId = server.Id;
        ServerName = server.Name;
        if(!string.IsNullOrEmpty(ServerName))
            ServerFirstLetter = ServerName.First().ToString().ToUpper();

        TimetrackingPermissions = new PermissionsTimetrackingModel(server.CanEditBillable,server.CanEditExport,server.CanEditRate,server.HasGitlabPlugin);
        StateChanged();
    }

    public void SetUserAndOffset(UserEntity user,TimeSpan offset)
    {
        ActualUser = user;
        TimeOffset = offset;
    }
  


    public void SetIsAuthenticated()
    {
        IsAuthenticated = true;
    }
    public void Disconnect()
    {
        IsAuthenticated = false;
        UserName= string.Empty;
        ApiPassword= string.Empty;
        BaseUrl= string.Empty;
        ActualUser = null;
        TimetrackingPermissions = null;
        ServerName = null;
        ServerFirstLetter = string.Empty;
        StateChanged();
    }

    private void StateChanged()
    { 
        OnPropertyChanged(nameof(TimetrackingPermissions));
        OnPropertyChanged(nameof(UserName));
        OnPropertyChanged(nameof(BaseUrl));
        OnPropertyChanged(nameof(ActualUser));
        OnPropertyChanged(nameof(IsAuthenticated));
        OnPropertyChanged(nameof(ServerName));
    }

}

﻿using CommunityToolkit.Mvvm.Messaging;
using MauiForKimai.Core;
using MauiForKimai.Interfaces;
using MauiForKimai.Messenger;
using MauiForKimai.Wrappers;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ViewModels;
public partial class FavouritesListViewModel : ViewModelBase, IViewModelSingleton
{
    private readonly IFavouritesTimesheetService _favouritesTimesheetService;
    public FavouritesListViewModel(IRoutingService rs, ILoginService ls, IFavouritesTimesheetService fts) : base(rs, ls)
    {
        _favouritesTimesheetService = fts;
        RegisterMessages();
    }

    private void RegisterMessages()
	{ 
		 WeakReferenceMessenger.Default.Register<TimesheetFavouriteCreateMessage>(this, (r, m) =>
        {
            Favourites.Insert(0,m.Value);
        });

         WeakReferenceMessenger.Default.Register<FavouritesRefreshMessage>(this, async (r, m) =>
        {
            await Refresh();
        });

        WeakReferenceMessenger.Default.Register<RefreshMessage>(this, async (r, m) =>
        {
            await Refresh();

        });

    }

    public override async Task Initialize()
    {
       await Refresh();
    }


    public ObservableCollection<TimesheetModel> Favourites {get; set; } = new();

    [RelayCommand]
    async Task Refresh()
    { 
        IsBusy = true;
        Favourites.Clear();
        if(base.LoginContext.IsAuthenticated)
        { 
            var favourites = await _favouritesTimesheetService.GetAll();
            foreach (var favourite in favourites)
            {
                Favourites.Add(favourite.ToTimesheetModel());
            }
        }
        IsBusy = false;
    }

    [RelayCommand]
    async Task AddNew()
    { 
        var route = base.routingService.GetRouteByViewModel<FavouritesDetailViewModel>();
        var wrapper = new TimesheetDetailWrapper(null,TimesheetDetailMode.Create);
		await Navigation.NavigateTo(route,wrapper);
    }

    [RelayCommand]
    async Task FavouriteTapped(TimesheetModel model)
    { 
        var route = base.routingService.GetRouteByViewModel<FavouritesDetailViewModel>();
        var wrapper = new TimesheetDetailWrapper(model,TimesheetDetailMode.Edit);
		await Navigation.NavigateTo(route, wrapper);
    }

    [RelayCommand]
    async Task QuickStart(TimesheetModel model)
    { 
        model.Begin =  DateTime.Now;
        WeakReferenceMessenger.Default.Send(new TimesheetStartExistingMessage(model));
        var route = base.routingService.GetRouteByViewModel<HomeViewModel>();
		await Navigation.NavigateTo(route, model);
    }

    


}

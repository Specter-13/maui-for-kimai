﻿using CommunityToolkit.Mvvm.Messaging;
using MauiForKimai.Messenger;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ViewModels;
public partial class TimesheetFavouritesListViewModel : ViewModelBase
{
    private readonly IFavouritesTimesheetService _favouritesTimesheetService;
    public TimesheetFavouritesListViewModel(IRoutingService rs, ILoginService ls, IFavouritesTimesheetService fts) : base(rs, ls)
    {
        _favouritesTimesheetService = fts;
        RegisterMessages();
    }



    private void RegisterMessages()
	{ 
		 WeakReferenceMessenger.Default.Register<TimesheetFavouriteCreateMessage>(this, async (r, m) =>
        {
            Favourites.Insert(0,m.Value);
            //Chosen = m.Value;
            
        });
	}

    [ObservableProperty]
    public TimesheetFavouritesListModel chosen;


    public ObservableCollection<TimesheetFavouritesListModel> Favourites {get; set; } = new();

    public override async Task Initialize()
    {
        IsBusy = true;
        var favourites = await _favouritesTimesheetService.GetAll();
        foreach (var favourite in favourites)
        {
            Favourites.Add((TimesheetFavouritesListModel)favourite);
        }
        IsBusy = false;
    }

    [RelayCommand]
    async Task AddNew()
    { 
        var route = base.routingService.GetRouteByViewModel<TimesheetFavouritesCreateViewModel>();
		await Navigation.NavigateTo(route);
    }

    [RelayCommand]
    async Task FavouriteTapped(TimesheetFavouritesListModel model)
    { 
       Chosen = model;
    }

    [RelayCommand]
    async Task UnsetChosen()
    { 
       Chosen = null;
    }


}

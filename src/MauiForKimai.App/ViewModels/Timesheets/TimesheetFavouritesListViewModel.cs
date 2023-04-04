using CommunityToolkit.Mvvm.Messaging;
using MauiForKimai.Interfaces;
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
        });


    }




    public ObservableCollection<TimesheetListItemModel> Favourites {get; set; } = new();

    public override async Task Initialize()
    {
        IsBusy = true;
        var favourites = await _favouritesTimesheetService.GetAll();
        foreach (var favourite in favourites)
        {
            Favourites.Add((TimesheetListItemModel)favourite);
        }
        IsBusy = false;
    }

    [RelayCommand]
    async Task Refresh()
    { 
       IsBusy = true;
        var favourites = await _favouritesTimesheetService.GetAll();
        Favourites.Clear();
        foreach (var favourite in favourites)
        {
            Favourites.Add((TimesheetListItemModel)favourite);
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
    async Task FavouriteTapped(TimesheetListItemModel model)
    { 
       //Chosen = model;
        var route = base.routingService.GetRouteByViewModel<TimesheetFavouritesCreateViewModel>();
		await Navigation.NavigateTo(route, model);
    }

    


}

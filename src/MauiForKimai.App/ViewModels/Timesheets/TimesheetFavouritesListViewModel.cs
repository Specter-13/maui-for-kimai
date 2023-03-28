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
        //var entity = new TimesheetFavouriteEntity
        //{
        //    Name = "UiCreation",
        //    ActivityId = 1,
        //    ActivityName = "Activity",
        //    ProjectId = 1,
        //    ProjectName = "Project",
        //    Exported=null,
        //    Billable = null,
        //    FixedRate= null,
        //    HourlyRate= null,
        //};
        //var listModel = (TimesheetFavouritesListModel) await _favouritesTimesheetService.Create(entity);

        //Favourites.Insert(0,listModel);
    }

    [RelayCommand]
    async Task FavouriteTapped(TimesheetFavouritesListModel model)
    { 
       Chosen = model;
    }

    [RelayCommand]
    async Task UnsetChosen(TimesheetFavouritesListModel model)
    { 
       Chosen = null;
    }

    
    //[RelayCommand]
    //async Task UnsetChosen(TimesheetFavouritesListModel model)
    //{ 
    //   Chosen = null;
    //}


}

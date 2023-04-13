using CommunityToolkit.Mvvm.Messaging;
using MauiForKimai.Core;
using MauiForKimai.Core.Entities;
using MauiForKimai.Messenger;
using MauiForKimai.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ViewModels;
public partial class FavouritesDetailViewModel : ViewModelBase
{
        private readonly IFavouritesTimesheetService _favouritesTimesheetService;
    public FavouritesDetailViewModel(IRoutingService rs, ILoginService ls, IFavouritesTimesheetService fts) : base(rs, ls)
    {
       _favouritesTimesheetService = fts;
        RegisterMessages();
    }


    private void RegisterMessages()
    {
        WeakReferenceMessenger.Default.Register<ItemChooseMessage,int>(this, (int)ChooseItemMode.Favourite, (r, m) =>
        {

            if (m.Value.ChooseItem is CustomerListModel customer)
            {
                ChosenCustomer = customer;
                if(! string.IsNullOrEmpty(ChosenProject.Name) )
                    ChosenProject = new();
            }

            if (m.Value.ChooseItem is ProjectListModel project)
            {
                ChosenProject = project;
                if(! string.IsNullOrEmpty(ChosenActivity.Name) )
                    ChosenActivity = new();
            }

            if (m.Value.ChooseItem is ActivityListModel activity)
            {
                ChosenActivity = activity;
            }

        });
        
       
    }

    [ObservableProperty]
    bool isEdit;

    public override Task OnParameterSet()
    {
        if (NavigationParameter is TimesheetDetailWrapper wrapper)
        { 
            if(wrapper.Mode == TimesheetDetailMode.Edit)
            { 
                IsEdit = true;

                var model = wrapper.Timesheet;
                ChosenCustomer = new CustomerListModel(model.CustomerId.GetValueOrDefault(), model.CustomerName, model.Billable.GetValueOrDefault());
                ChosenActivity =  new ActivityListModel(model.ActivityId, model.ActivityName, model.Billable);
                ChosenProject = new ProjectListModel(model.ProjectId, model.ProjectName, model.CustomerId.GetValueOrDefault(), model.Billable);

                Favourite = model.ToTimesheetFavouriteEntity();
            }

        }
            return base.OnParameterSet();
    }

    [ObservableProperty]
    public TimesheetFavouriteEntity favourite = new();

    [ObservableProperty]
    string selectedBillableMode = "Automatic";

    [ObservableProperty]
    CustomerListModel chosenCustomer = new();

    [ObservableProperty]
    ProjectListModel chosenProject = new();

    [ObservableProperty]
    ActivityListModel chosenActivity= new();

    [RelayCommand]
    async Task ShowProjectChooseView()
    {
        var route = routingService.GetRouteByViewModel<ProjectChooseFavouriteViewModel>();
        var wrapper = new ChooseItemWrapper(ChosenProject,ChooseItemMode.Favourite);
        wrapper.ChosenCustomerId = ChosenCustomer.Id;
        await Navigation.NavigateTo(route, wrapper);
    }

    [RelayCommand]
    async Task ShowActivityChooseView()
    {
        var route = routingService.GetRouteByViewModel<CustomerChooseFavouriteViewModel>();
        var wrapper = new ChooseItemWrapper(ChosenActivity,ChooseItemMode.Favourite);
        wrapper.ChosenProjectId = ChosenProject.Id;
        await Navigation.NavigateTo(route, wrapper);
    }
    
    [RelayCommand]
    async Task ShowCustomerChooseView()
    {
        var route = routingService.GetRouteByViewModel<CustomerChooseFavouriteViewModel>();
        var wrapper = new ChooseItemWrapper(ChosenCustomer,ChooseItemMode.Favourite);
        await Navigation.NavigateTo(route, wrapper);
    }

    [RelayCommand]
    async Task Save()
    {
        Favourite.CustomerName = ChosenCustomer.Name;
        Favourite.ActivityId = ChosenActivity.Id;
        Favourite.ActivityName = ChosenActivity.Name;
        Favourite.ProjectName = ChosenProject.Name;
        Favourite.ProjectId = ChosenProject.Id;
       
        await _favouritesTimesheetService.Update(Favourite);
        WeakReferenceMessenger.Default.Send(new FavouritesRefreshMessage(string.Empty));
        await Navigation.NavigateTo("..");
    }

    [RelayCommand]
    async Task Create()
    {
        Favourite.CustomerName = ChosenCustomer.Name;
        Favourite.ActivityId = ChosenActivity.Id;
        Favourite.ActivityName = ChosenActivity.Name;
        Favourite.ProjectName = ChosenProject.Name;
        Favourite.ProjectId = ChosenProject.Id;
       
        await _favouritesTimesheetService.Create(Favourite);
        WeakReferenceMessenger.Default.Send(new FavouritesRefreshMessage(string.Empty));
        await Navigation.NavigateTo("..");
    }
    [RelayCommand]
    async Task Delete()
    {

        await _favouritesTimesheetService.Delete(Favourite.Id);
        WeakReferenceMessenger.Default.Send(new FavouritesRefreshMessage(string.Empty));
        await Navigation.NavigateTo("..");
    }

}

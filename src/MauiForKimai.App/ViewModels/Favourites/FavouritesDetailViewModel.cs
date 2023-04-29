using CommunityToolkit.Mvvm.Messaging;
using MauiForKimai.Core;
using MauiForKimai.Core.Entities;
using MauiForKimai.Core.Validators;
using MauiForKimai.Messenger;
using MauiForKimai.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ViewModels;
public partial class FavouritesDetailViewModel : ViewModelBase, IViewModelTransient
{
    private readonly IFavouritesTimesheetService _favouritesTimesheetService;
    public FavouritesDetailViewModel(IRoutingService rs, ILoginService ls, IFavouritesTimesheetService fts) : base(rs, ls)
    {
       _favouritesTimesheetService = fts;
    }

    private void HandleReceivedChosenItem(ChooseItemWrapper wrapper)
    { 
        if (wrapper.ChooseItem is CustomerListModel customer)
        {
            ChosenCustomer = customer;
            if(! string.IsNullOrEmpty(ChosenProject.Name) )
                ChosenProject = new();
        }
        else if (wrapper.ChooseItem is ProjectListModel project)
        {
            ChosenProject = project;
            if(! string.IsNullOrEmpty(ChosenActivity.Name) )
                ChosenActivity = new();
        }
        else if (wrapper.ChooseItem is ActivityListModel activity)
        {
            ChosenActivity = activity;
        }
    }

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
        else if (NavigationParameter is ChooseItemWrapper chooseWrapper)
        {
            HandleReceivedChosenItem(chooseWrapper);
        }
            return base.OnParameterSet();
    }

    [ObservableProperty]
    bool isEdit;

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

    [ObservableProperty]
    public string validationErrors;

    [ObservableProperty]
    public bool showErrors;

    private TimesheetFavouriteEntityValidator _validator = new ();
    [RelayCommand]
    async Task ShowProjectChooseView()
    {
        var route = routingService.GetRouteByViewModel<ProjectChooseFavouriteViewModel>();
        var wrapper = new ChooseItemWrapper(ChosenProject);
        wrapper.ChosenCustomerId = ChosenCustomer.Id;
        await Navigation.NavigateTo(route, wrapper);
    }

    [RelayCommand]
    async Task ShowActivityChooseView()
    {
        var route = routingService.GetRouteByViewModel<CustomerChooseFavouriteViewModel>();
        var wrapper = new ChooseItemWrapper(ChosenActivity);
        wrapper.ChosenProjectId = ChosenProject.Id;
        await Navigation.NavigateTo(route, wrapper);
    }
    
    [RelayCommand]
    async Task ShowCustomerChooseView()
    {
        var route = routingService.GetRouteByViewModel<CustomerChooseFavouriteViewModel>();
        var wrapper = new ChooseItemWrapper(ChosenCustomer);
        await Navigation.NavigateTo(route, wrapper);
    }

    [RelayCommand]
    async Task Save()
    {
        ShowErrors = false;
        ValidationErrors = string.Empty;
        Favourite.CustomerName = ChosenCustomer.Name;
        Favourite.ActivityId = ChosenActivity.Id;
        Favourite.ActivityName = ChosenActivity.Name;
        Favourite.ProjectName = ChosenProject.Name;
        Favourite.ProjectId = ChosenProject.Id;
       
        var result = _validator.Validate(Favourite);
        if(result.IsValid)
        { 
            await _favouritesTimesheetService.Update(Favourite);
            WeakReferenceMessenger.Default.Send(new FavouritesRefreshMessage(string.Empty));
            await Navigation.NavigateTo("..");
        }
        else
        { 
            ValidationErrors = result.ToString("\n");
            ShowErrors = true;
        }
    }

    [RelayCommand]
    async Task Create()
    {
        ShowErrors = false;
        ValidationErrors = string.Empty;
        Favourite.CustomerName = ChosenCustomer.Name;
        Favourite.CustomerId = ChosenCustomer.Id;
        Favourite.ActivityId = ChosenActivity.Id;
        Favourite.ActivityName = ChosenActivity.Name;
        Favourite.ProjectName = ChosenProject.Name;
        Favourite.ProjectId = ChosenProject.Id;
       

        var result = _validator.Validate(Favourite);
        if(result.IsValid)
        { 
            await _favouritesTimesheetService.Create(Favourite);
            WeakReferenceMessenger.Default.Send(new FavouritesRefreshMessage(string.Empty));
            await Navigation.NavigateTo("..");
        }
        else
        { 
            ValidationErrors = result.ToString("\n");
            ShowErrors = true;
        }
    }

    [RelayCommand]
    async Task StartFavourite()
    { 
        Favourite.ActivityId = ChosenActivity.Id;
        Favourite.ProjectId = ChosenProject.Id;
        Favourite.CustomerId = ChosenCustomer.Id;

        var model = Favourite.ToTimesheetModel();
        model.Begin = DateTime.Now;
        model.End = null;
        WeakReferenceMessenger.Default.Send(new TimesheetStartExistingMessage(model));
        var route = base.routingService.GetRouteByViewModel<HomeViewModel>();
		await Navigation.NavigateTo(route, model);
    }


    [RelayCommand]
    async Task Delete()
    {

        await _favouritesTimesheetService.Delete(Favourite.Id);
        WeakReferenceMessenger.Default.Send(new FavouritesRefreshMessage(string.Empty));
        await Navigation.NavigateTo("..");
    }

}

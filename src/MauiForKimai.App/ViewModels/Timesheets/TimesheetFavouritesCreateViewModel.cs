using CommunityToolkit.Mvvm.Messaging;
using MauiForKimai.Core.Entities;
using MauiForKimai.Messenger;
using MauiForKimai.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ViewModels;
public partial class TimesheetFavouritesCreateViewModel : ViewModelBase
{
    public TimesheetFavouritesCreateViewModel(IRoutingService rs, ILoginService ls) : base(rs, ls)
    {
       
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



            //ChosenProject = m.Value;
            //Favourite.ProjectId = ChosenProject.Id; ;
            //Favourite.ProjectName = ChosenProject.Name; ;
            //IsProjectNotValid = false;
        });

        //WeakReferenceMessenger.Default.Register<TimesheetActivityChooseMessage>(this, (r, m) =>
        //{
        //    ChosenActivity = m.Value;
        //    Timesheet.Activity = ChosenActivity.Id;
        //    IsActivityNotValid = false;
        //});

        //WeakReferenceMessenger.Default.Register<ChooseItemWrapper>(this, (r, m) =>
        //{
        //    ChosenCustomer = (CustomerListModel)m.ChooseItem;
        //    Favourite.CustomerName = ChosenCustomer.Name;
        //    // validation, that user never pick project, which customer do not contain
        //    if(ChosenProject != null) ChosenProject = null;
        //});
    }

    public override Task OnDisappearing()
    {
        //WeakReferenceMessenger.Default.Unregister<ItemChooseMessage>(this);
        //WeakReferenceMessenger.Default.Unregister<TimesheetActivityChooseMessage>(this);
        //WeakReferenceMessenger.Default.Unregister<TimesheetCustomerChooseMessage>(this);
        return base.OnDisappearing();
    }

    [ObservableProperty]
    public TimesheetFavouriteEntity favourite;

    [ObservableProperty]
    CustomerListModel chosenCustomer = new();

    [ObservableProperty]
    ProjectListModel chosenProject = new();

    [ObservableProperty]
    ActivityListModel chosenActivity= new();

    [RelayCommand]
    async Task ShowProjectChooseView()
    {
        //var route = routingService.GetRouteByViewModel<ProjecChooseFavouriteViewModel>();
        //ChosenCustomer.Mode = ChooseTimesheetMode.Favourite;
        //await Navigation.NavigateTo(route,ChosenCustomer);

        var route = routingService.GetRouteByViewModel<ProjectChooseFavouriteViewModel>();
        var wrapper = new ChooseItemWrapper(ChosenProject,ChooseItemMode.Favourite);
        wrapper.ChosenCustomerId = ChosenCustomer.Id;
        await Navigation.NavigateTo(route, wrapper);
    }

    [RelayCommand]
    async Task ShowActivityChooseView()
    {
        var route = routingService.GetRouteByViewModel<CustomerChooseTimesheetViewModel>();
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

}

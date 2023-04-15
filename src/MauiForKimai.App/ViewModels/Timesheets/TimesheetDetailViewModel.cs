using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
using FluentValidation.Results;
using MauiForKimai.ApiClient;
using MauiForKimai.Core.Validators;
using MauiForKimai.Messenger;
using MauiForKimai.Popups;
using MauiForKimai.Wrappers;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace MauiForKimai.ViewModels;



public partial class TimesheetDetailViewModel : ViewModelBase
{
    private readonly ICustomerService _customerService;
    private readonly ITimesheetService _timesheetService;
    private readonly PopupSizeConstants _popupSizeConstants;
    private readonly IFavouritesTimesheetService  _favouriteTimesheetService;
    public TimesheetDetailViewModel(IRoutingService rs,
        ILoginService ls, 
        ICustomerService customerService,
        ITimesheetService ts, PopupSizeConstants sc, IFavouritesTimesheetService fts) : base(rs, ls)
    {
        RegisterMessages();
        _customerService = customerService;
        _timesheetService = ts;
        _popupSizeConstants = sc;
        _favouriteTimesheetService = fts;

        
    }

    private void RegisterMessages()
    { 
         WeakReferenceMessenger.Default.Register<ItemChooseMessage,int>(this, (int)ChooseItemMode.Timesheet, (r, m) =>
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
    private int _id;

   
    public override Task OnParameterSet()
    {

        if (NavigationParameter is TimesheetDetailWrapper wrapper)
        {
            PageLabel = "Edit";
            _id = wrapper.Timesheet.Id;
            
            Mode = wrapper.Mode;

            var timesheetModel = wrapper.Timesheet;
            //TODO check for roles
            Timesheet = timesheetModel;

            ChosenActivity = new ActivityListModel(timesheetModel.ActivityId,timesheetModel.ActivityName,timesheetModel.Billable.Value);
            ChosenProject = new ProjectListModel(timesheetModel.ProjectId,timesheetModel.ProjectName,timesheetModel.CustomerId.GetValueOrDefault(),timesheetModel.Billable.Value);
            ChosenCustomer = new CustomerListModel(timesheetModel.CustomerId.GetValueOrDefault(),timesheetModel.CustomerName, timesheetModel.Billable.Value );


            TimeWrapper = new TimeBeginEndWrapper(timesheetModel,LoginContext.TimeOffset);

        }
        else if (NavigationParameter is TimesheetDetailMode mode)
        {
            
            if(mode == TimesheetDetailMode.Start)
            { 
                PageLabel = "Start new";
                TimeWrapper = new TimeBeginEndWrapper(LoginContext.TimeOffset);
                Timesheet.Begin = TimeWrapper.BeginFull;
            }
            else
            {
                TimeWrapper = new TimeBeginEndWrapper(LoginContext.TimeOffset);
                Timesheet.Begin = TimeWrapper.BeginFull;
                PageLabel = "Create";
                Mode = TimesheetDetailMode.Create;
            }
            Mode = mode;
           
            
        }
     

        IsCreateOrEdit = Mode == TimesheetDetailMode.Edit || Mode == TimesheetDetailMode.Create;
        return base.OnParameterSet();
    }


 

    [ObservableProperty]
    string pageLabel;


    [ObservableProperty]
    TimeBeginEndWrapper timeWrapper;


    [ObservableProperty]
    TimesheetModel timesheet = new();

    private TimesheetModelStartValidator _startValidator = new ();
    private TimesheetModelCreateValidator _createValidator = new ();
    [ObservableProperty]
    CustomerListModel chosenCustomer = new();

    [ObservableProperty]
    ProjectListModel chosenProject = new();

    [ObservableProperty]
    ActivityListModel chosenActivity = new();
    
    [ObservableProperty]
    string selectedBillableMode = "Automatic";

    [ObservableProperty]
    int gitlabIssueId;
   

    [ObservableProperty]
    TimesheetDetailMode mode;

    [ObservableProperty]
    bool isCreateOrEdit;

    [RelayCommand]
    async Task ShowProjectChooseView()
    {
        var route = routingService.GetRouteByViewModel<ProjectChooseRecentTimesheetViewModel>();
        var wrapper = new ChooseItemWrapper(ChosenProject,ChooseItemMode.Timesheet);
        wrapper.ChosenCustomerId = ChosenCustomer.Id;
        await Navigation.NavigateTo(route, wrapper);
    }

    [RelayCommand]
    async Task ShowActivityChooseView()
    {
        var route = routingService.GetRouteByViewModel<CustomerChooseRecentTimesheetViewModel>();
        var wrapper = new ChooseItemWrapper(ChosenActivity,ChooseItemMode.Timesheet);
        wrapper.ChosenProjectId = ChosenProject.Id;
        await Navigation.NavigateTo(route, wrapper);
    }

    [RelayCommand]
    async Task ShowCustomerChooseView()
    {
        var route = routingService.GetRouteByViewModel<CustomerChooseRecentTimesheetViewModel>();
        var wrapper = new ChooseItemWrapper(ChosenCustomer,ChooseItemMode.Timesheet);
        await Navigation.NavigateTo(route, wrapper);
    }

  

    [RelayCommand]
    async Task AddToFavourites()
    {

        await _favouriteTimesheetService.Create(Timesheet.ToTimesheetFavouriteEntity());
        await Toast.Make("Timesheet added to favourites!", ToastDuration.Short, 14).Show();
        WeakReferenceMessenger.Default.Send(new FavouritesRefreshMessage(""));
    }

    [ObservableProperty]
    List<string> validationErrors;

 
    [RelayCommand]
    async Task StartTimesheet()
    {
        //TODO higher roles
        Timesheet.Begin = TimeWrapper.BeginFull;
        Timesheet.ProjectId = ChosenProject.Id;
        Timesheet.ProjectName = ChosenProject.Name;
        Timesheet.ActivityId = ChosenActivity.Id;
        Timesheet.CustomerName = ChosenCustomer.Name;


        ValidationResult result;
        if(Mode == TimesheetDetailMode.Create || Mode == TimesheetDetailMode.Edit)
        {
            result = _createValidator.Validate(Timesheet);
        }
        else
        { 
            result = _startValidator.Validate(Timesheet);
        }

        if (result.IsValid)
        {
            //Save the data...
            if (base.LoginContext.TimetrackingPermissions.CanEditBillable)
            {
                await SetBillable();
            }


            var wrapper = new TimesheetTimetrackingWrapper(Timesheet, ChosenActivity.Name, ChosenProject.Name, GitlabIssueId);
            WeakReferenceMessenger.Default.Send(new TimesheetStartNewMessage(wrapper));
            await Navigation.NavigateTo("..");
        }
        else
        {
            ValidationErrors = result.Errors.Select(x => x.ErrorMessage).ToList();
            
            // Display the validation errors...
        }




    }

    [RelayCommand]
    async Task DurationTapped()
    { 
        var duration = await DisplayDurationPopup();
        if(duration == null) return;
        TimeWrapper.UpdateEnd(duration.Value);


    }
    private async Task<TimeSpan?> DisplayDurationPopup()
    {
        var popup = new DurationPopup(_popupSizeConstants, new DurationPopupViewModel(TimeWrapper.Duration));

        return (TimeSpan?) await Page.ShowPopupAsync(popup);
    }

    [RelayCommand]
    async Task Cancel()
    {
        await Navigation.NavigateTo("..");
    }

    [RelayCommand]
    async Task Save()
    {
        Timesheet.Begin = TimeWrapper.BeginFull;
        Timesheet.End = TimeWrapper.EndFull;
        Timesheet.ProjectId = ChosenProject.Id;
        Timesheet.ActivityId = ChosenActivity.Id;
        Timesheet.ProjectName = ChosenProject.Name;
        Timesheet.ActivityName = ChosenActivity.Name;

        var result = _createValidator.Validate(Timesheet);
        if(result.IsValid)
        { 
            await _timesheetService.Update(_id,Timesheet.ToTimesheetEditForm(base.LoginContext.TimetrackingPermissions, LoginContext.TimeOffset));
            //TODO = roles
            await Navigation.NavigateTo("..");
        }
        else
        {
           ValidationErrors = result.Errors.Select(x => x.ErrorMessage).ToList();
        }
     }


        [RelayCommand]
    async Task Delete()
    {
        await _timesheetService.Delete(_id);
        //TODO = roles
        await Navigation.NavigateTo("..");
    }

    [RelayCommand]
    async Task Create()
    {

        Timesheet.Begin = TimeWrapper.BeginFull;
        Timesheet.End = TimeWrapper.EndFull;
        Timesheet.ProjectId = ChosenProject.Id;
        Timesheet.ActivityId = ChosenActivity.Id;
        Timesheet.ProjectName = ChosenProject.Name;
        Timesheet.ActivityName = ChosenActivity.Name;

        var result = _createValidator.Validate(Timesheet);

        if(result.IsValid)
        { 
            await _timesheetService.Create(Timesheet.ToTimesheetEditForm(base.LoginContext.TimetrackingPermissions, LoginContext.TimeOffset));
            await Navigation.NavigateTo("..");
        }
        else
        {
            ValidationErrors = result.Errors.Select(x => x.ErrorMessage).ToList();
        }
    }
    //TODO - createa own validation object
  


    private async Task SetBillable()
    { 
        if (SelectedBillableMode == "Automatic")
        {
            bool isCustomerBillable = false;
            if(ChosenCustomer == null)
            {
                var customer = await _customerService.GetById(ChosenProject.CustomerId); 
                if(customer != null) 
                {
                    isCustomerBillable = customer.Billable;
                }
            
            }
            else
            {
                isCustomerBillable = ChosenCustomer.Billable;
            }
            Timesheet.Billable = ChosenProject.Billable.Value && ChosenActivity.Billable.Value && isCustomerBillable;
        }
        else if (SelectedBillableMode == "Yes")
        { 
            Timesheet.Billable = true;
        }
        else
        { 
            Timesheet.Billable = false;
        }
        
    }


}


public class TimesheetDetailAllViewModel : IViewModel
{
}
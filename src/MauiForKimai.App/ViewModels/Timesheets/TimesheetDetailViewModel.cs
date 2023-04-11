using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
using MauiForKimai.ApiClient.Validators;
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
            Timesheet = timesheetModel.ToTimesheetEditForm(LoginContext.TimetrackingPermissions);

            ChosenActivity = new ActivityListModel(timesheetModel.ActivityId,timesheetModel.ActivityName,timesheetModel.Billable.Value);
            ChosenProject = new ProjectListModel(timesheetModel.ProjectId,timesheetModel.ProjectName,timesheetModel.CustomerId,timesheetModel.Billable.Value);
            ChosenCustomer = new CustomerListModel(timesheetModel.CustomerId,timesheetModel.CustomerName, timesheetModel.Billable.Value );


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
    TimesheetEditForm timesheet = new();

    private TimesheetEditFormValidator _validator = new ();

    [ObservableProperty]
    CustomerListModel chosenCustomer = new();

    [ObservableProperty]
    ProjectListModel chosenProject = new();

    [ObservableProperty]
    ActivityListModel chosenActivity = new();
    
    [ObservableProperty]
    string selectedBillableMode = "Automatic";

   

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
        var entity = new TimesheetFavouriteEntity{
            ActivityId = ChosenActivity.Id,
            ActivityName = ChosenActivity.Name,
            ProjectId = ChosenProject.Id,
            ProjectName = ChosenProject.Name,
            CustomerName = ChosenCustomer.Name,
            Description = Timesheet.Description,
            Tags = Timesheet.Tags

        };

        await _favouriteTimesheetService.Create(entity);
        await Toast.Make("Timesheet added to favourites!", ToastDuration.Short, 14).Show();
        WeakReferenceMessenger.Default.Send(new FavouritesRefreshMessage(""));
    }

    [ObservableProperty]
    List<string> validationErrors;

    [ObservableProperty]
    bool showValidationErrors;
    
    [RelayCommand]
    async Task StartTimesheet()
    {
        //TODO higher roles
        Timesheet.Begin = TimeWrapper.BeginFull.ToDateTimeOffset(LoginContext.TimeOffset);
        Timesheet.Project = ChosenProject.Id;
        Timesheet.Activity = ChosenActivity.Id;

     
        var result = _validator.Validate(Timesheet);

        if (result.IsValid)
        {
            // Save the data...
            if (base.LoginContext.TimetrackingPermissions.CanEditBillable)
            {
                await SetBillable();
            }


            var wrapper = new TimesheetTimetrackingWrapper(Timesheet,ChosenActivity.Name,ChosenProject.Name);
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
        Timesheet.Begin = TimeWrapper.BeginFull.ToDateTimeOffset(LoginContext.TimeOffset);
        Timesheet.End = TimeWrapper.EndFull.ToDateTimeOffset(LoginContext.TimeOffset);
        Timesheet.Project = ChosenProject.Id;
        Timesheet.Activity = ChosenActivity.Id;

        await _timesheetService.Update(_id,Timesheet);
        //TODO = roles
        await Navigation.NavigateTo("..");
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
        Timesheet.Project = ChosenProject.Id;
        Timesheet.Activity = ChosenActivity.Id;

        await _timesheetService.Create(Timesheet);
        //TODO = roles
        await Navigation.NavigateTo("..");
    }
    //TODO - createa own validation object
  


    private async Task SetBillable()
    { 
        if (SelectedBillableMode == "Automatic")
        {
            bool? isCustomerBillable;
            if(ChosenCustomer == null)
            {
                isCustomerBillable = (await _customerService.GetById(ChosenProject.CustomerId)).Billable; 
            }
            else
            {
                isCustomerBillable = ChosenCustomer.Billable;
            }
            Timesheet.Billable = ChosenProject.Billable.Value && ChosenActivity.Billable.Value && isCustomerBillable.Value;
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
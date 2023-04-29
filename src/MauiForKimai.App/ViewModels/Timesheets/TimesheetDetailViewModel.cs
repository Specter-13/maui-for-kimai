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



namespace MauiForKimai.ViewModels;



public partial class TimesheetDetailViewModel : ViewModelBase, IViewModelTransient
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
        _customerService = customerService;
        _timesheetService = ts;
        _popupSizeConstants = sc;
        _favouriteTimesheetService = fts;

        
    }
    
    private int _id;

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
        else if (NavigationParameter is ChooseItemWrapper chooseWrapper)
        {
            HandleReceivedChosenItem(chooseWrapper);
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

   public TimesheetModelStartValidator _startValidator = new ();
    TimesheetModelCreateValidator _createValidator = new ();

    [ObservableProperty]
    CustomerListModel chosenCustomer = new();

    [ObservableProperty]
    ProjectListModel chosenProject = new();

    [ObservableProperty]
    ActivityListModel chosenActivity = new();
    
    [ObservableProperty]
    string selectedBillableMode = "Automatic";

    [ObservableProperty]
    string validationErrors;

    [ObservableProperty]
    int gitlabIssueId;
   

    [ObservableProperty]
    TimesheetDetailMode mode;

    [ObservableProperty]
    bool isCreateOrEdit;

    [RelayCommand]
    async Task ShowProjectChooseView()
    {
        var route = GetProjectChooseRoute();
        var wrapper = new ChooseItemWrapper(ChosenProject);
        wrapper.ChosenCustomerId = ChosenCustomer.Id;
        await Navigation.NavigateTo(route, wrapper);
    }

   

    [RelayCommand]
    async Task ShowActivityChooseView()
    {
        var route = GetActivityChooseRoute();
      
        var wrapper = new ChooseItemWrapper(ChosenActivity);
        wrapper.ChosenProjectId = ChosenProject.Id;
        await Navigation.NavigateTo(route, wrapper);
    }

    [RelayCommand]
    async Task ShowCustomerChooseView()
    {
        var route = GetCustomerChooseRoute();
        var wrapper = new ChooseItemWrapper(ChosenCustomer);
        await Navigation.NavigateTo(route, wrapper);
    }

    private string GetActivityChooseRoute()
    { 
        if(Mode == TimesheetDetailMode.Create || Mode == TimesheetDetailMode.Start || Timesheet.IsRecent)
            return routingService.GetRouteByViewModel<ActivityChooseRecentTimesheetViewModel>();
        else
        { 
            return routingService.GetRouteByViewModel<ActivityChooseDetailAllViewModel>();
        }
    }
    private string GetProjectChooseRoute()
    { 
        if(Mode == TimesheetDetailMode.Create || Mode == TimesheetDetailMode.Start || Timesheet.IsRecent)
            return routingService.GetRouteByViewModel<ProjectChooseRecentTimesheetViewModel>();
        else
        { 
            return routingService.GetRouteByViewModel<ProjectChooseDetailAllViewModel>();
        }
    }
    private string GetCustomerChooseRoute()
    { 
        if(Mode == TimesheetDetailMode.Create || Mode == TimesheetDetailMode.Start || Timesheet.IsRecent)
            return routingService.GetRouteByViewModel<CustomerChooseRecentTimesheetViewModel>();
        else
        { 
            return routingService.GetRouteByViewModel<CustomerChooseDetailAllViewModel>();
        }
    }

    [RelayCommand]
    async Task AddToFavourites()
    {

        await _favouriteTimesheetService.Create(Timesheet.ToTimesheetFavouriteEntity());
        await Toast.Make("Timesheet added to favourites!", ToastDuration.Short, 14).Show();
        WeakReferenceMessenger.Default.Send(new FavouritesRefreshMessage(""));
    }


 
    [RelayCommand]
    async Task StartTimesheet()
    {
        ValidationErrors = string.Empty;
        Timesheet.ProjectId = ChosenProject.Id;
        Timesheet.ProjectName = ChosenProject.Name;
        Timesheet.ActivityId = ChosenActivity.Id;
        Timesheet.CustomerName = ChosenCustomer.Name;

        Timesheet.Begin = DateTime.Now;
        Timesheet.End = null;

        ValidationResult result;
        result = _startValidator.Validate(Timesheet);
        

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
            ValidationErrors = string.Empty;
        }
        else
        {
           ValidationErrors =  result.ToString("\n");
           await Toast.Make("Form validation failed!", ToastDuration.Short, 14).Show();
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
        ValidationErrors =  string.Empty;
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
            ValidationErrors = string.Empty;
        }
        else
        {
           await Toast.Make("Form validation failed!", ToastDuration.Short, 14).Show();
           ValidationErrors =  result.ToString("\n");
        }
     }


    [RelayCommand]
    async Task Delete()
    {
        await _timesheetService.Delete(_id);
        await Navigation.NavigateTo("..", Timesheet);
    }

    [RelayCommand]
    async Task Create()
    {
        ValidationErrors =  string.Empty;
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
            ValidationErrors = string.Empty;
        }
        else
        {
            await Toast.Make("Form validation failed!", ToastDuration.Short, 14).Show();
            ValidationErrors =  result.ToString("\n");
        }
    }



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


public class TimesheetDetailAllViewModel : IViewModelTransient
{
}
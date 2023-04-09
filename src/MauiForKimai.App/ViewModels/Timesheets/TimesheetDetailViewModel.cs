using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
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


        if(ApiStateProvider.IsTeamlead) SelectedBillableMode = "Automatic";
        
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
            Timesheet = timesheetModel.ToTimesheetEditFormBase();

            ChosenActivity = new ActivityListModel(timesheetModel.ActivityId,timesheetModel.ActivityName,timesheetModel.Billable.Value);
            ChosenProject = new ProjectListModel(timesheetModel.ProjectId,timesheetModel.ProjectName,timesheetModel.CustomerId,timesheetModel.Billable.Value);
            ChosenCustomer = new CustomerListModel(timesheetModel.CustomerId,timesheetModel.CustomerName, timesheetModel.Billable.Value );


            TimeWrapper = new TimeBeginEndWrapper(timesheetModel,loginService.GetUserTimeOffset());

        }
        else if (NavigationParameter is TimesheetDetailMode mode)
        {
            
            if(mode == TimesheetDetailMode.Start)
            { 
                PageLabel = "Start new";
                TimeWrapper = new TimeBeginEndWrapper(loginService.GetUserTimeOffset());
                Timesheet.Begin = TimeWrapper.BeginFull;
            }
            else
            {
                TimeWrapper = new TimeBeginEndWrapper(loginService.GetUserTimeOffset());
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

    [ObservableProperty]
    CustomerListModel chosenCustomer = new();

    [ObservableProperty]
    ProjectListModel chosenProject = new();

    [ObservableProperty]
    ActivityListModel chosenActivity = new();
    
    [ObservableProperty]
    string selectedBillableMode;

    [ObservableProperty]
    bool isTagNotValid;

    [ObservableProperty]
    bool isProjectNotValid;

    [ObservableProperty]
    bool isActivityNotValid;

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
        var timesheet = await _favouriteTimesheetService.Create(entity);
        WeakReferenceMessenger.Default.Send(new FavouritesRefreshMessage(""));
    }

    [RelayCommand]
    async Task StartTimesheet()
    {
        //TODO higher roles
        Timesheet.Begin = TimeWrapper.BeginFull;
        Timesheet.Project = ChosenProject.Id;
        Timesheet.Activity = ChosenActivity.Id;

        //if(!Validate()) return;

        IsTagNotValid = false;
        IsProjectNotValid = false;
        IsActivityNotValid = false;

        //if roles is higher than teamlead, set billable value
        if(ApiStateProvider.IsTeamlead)
        {
           await SetBillable();
        }



        //Timesheet.User = base.ApiStateProvider.ActualUser.Id;

        //Timesheet.Billable = false;
        //Timesheet.Exported = false;
        //Timesheet.FixedRate = 0;
        //Timesheet.HourlyRate = 0;
        //Timesheet.P

        var wrapper = new TimesheetTimetrackingWrapper(Timesheet,ChosenActivity.Name,ChosenProject.Name);
        WeakReferenceMessenger.Default.Send(new TimesheetStartNewMessage(wrapper));
        await Navigation.NavigateTo("..");

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
    private bool Validate()
    {
        if(Timesheet.Tags != null && Timesheet.Tags.Length == 1) 
        { 
            IsTagNotValid = true;
        }

        if(Timesheet.Project == 0)
        {
            IsProjectNotValid = true;
        }

        if(Timesheet.Activity == 0)
        {
            IsActivityNotValid = true;
        }

        if(IsTagNotValid || IsProjectNotValid || IsActivityNotValid)
        {
            return false;
        }
        return true;
    }


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
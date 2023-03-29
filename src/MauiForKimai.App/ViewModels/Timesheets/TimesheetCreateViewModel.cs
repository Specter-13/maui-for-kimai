using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
using MauiForKimai.Messenger;
using MauiForKimai.Views.Timesheets;
using MauiForKimai.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MauiForKimai.ViewModels.Timesheets;

public partial class TimesheetCreateViewModel : ViewModelBase
{
    private readonly ICustomerService _customerService;

    public TimesheetCreateViewModel(IRoutingService rs,ILoginService ls, ICustomerService customerService) : base(rs, ls)
    {
        _customerService = customerService;
         RegisterMessages();

         Timesheet = new TimesheetEditForm();
        Timesheet.Begin = new DateTimeOffset(DateTime.Now);

        BeginTime = Timesheet.Begin.TimeOfDay;
        //EndTime = Timesheet.End.Value.TimeOfDay;

        BeginDate = Timesheet.Begin.Date;
        //EndDate = Timesheet.End.Value.Date;

        Duration = (EndTime - BeginTime).ToString(@"hh\:mm\:ss");
        BeginDateString = BeginDate.ToString("dddd, dd MMMM yyyy");

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



            //ChosenProject = m.Value;
            //Favourite.ProjectId = ChosenProject.Id; ;
            //Favourite.ProjectName = ChosenProject.Name; ;
            //IsProjectNotValid = false;
        });
    }


    [ObservableProperty]
    string duration;

    [ObservableProperty]
    string beginDateString;

    [ObservableProperty]
    DateTime beginDate;

    [ObservableProperty]
    DateTime endDate;

    [ObservableProperty]
    TimeSpan beginTime;

    [ObservableProperty]
    TimeSpan endTime;

    [ObservableProperty]
    TimesheetEditForm timesheet;

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


    [RelayCommand]
    async Task ShowProjectChooseView()
    {
        var route = routingService.GetRouteByViewModel<ProjectChooseTimesheetViewModel>();
        var wrapper = new ChooseItemWrapper(ChosenProject,ChooseItemMode.Timesheet);
        wrapper.ChosenCustomerId = ChosenCustomer.Id;
        await Navigation.NavigateTo(route, wrapper);
    }

    [RelayCommand]
    async Task ShowActivityChooseView()
    {
        var route = routingService.GetRouteByViewModel<CustomerChooseTimesheetViewModel>();
        var wrapper = new ChooseItemWrapper(ChosenActivity,ChooseItemMode.Timesheet);
        wrapper.ChosenProjectId = ChosenProject.Id;
        await Navigation.NavigateTo(route, wrapper);
    }

    [RelayCommand]
    async Task ShowCustomerChooseView()
    {
        var route = routingService.GetRouteByViewModel<CustomerChooseTimesheetViewModel>();
        var wrapper = new ChooseItemWrapper(ChosenCustomer,ChooseItemMode.Timesheet);
        await Navigation.NavigateTo(route, wrapper);
    }

    [RelayCommand]
    async Task StartTimesheet()
    {
        var startTime = new DateTimeOffset(BeginDate.Year, BeginDate.Month, BeginDate.Day, BeginTime.Hours, BeginTime.Minutes, BeginTime.Seconds, loginService.GetUserTimeOffset());
        //var end = new DateTimeOffset(EndDate.Year, EndDate.Month, EndDate.Day, EndTime.Hours, EndTime.Minutes, EndTime.Seconds, new TimeSpan(0,0,0));
        Timesheet.Begin = startTime;
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
        WeakReferenceMessenger.Default.Send(new TimesheetStartMessage(wrapper));
        //WeakReferenceMessenger.Default.Send
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
            bool isCustomerBillable;
            if(ChosenCustomer == null)
            {
                isCustomerBillable = (await _customerService.GetById(ChosenProject.CustomerId)).Billable; 
            }
            else
            {
                isCustomerBillable = ChosenCustomer.Billable;
            }
            Timesheet.Billable = ChosenProject.Billable && ChosenActivity.Billable && isCustomerBillable;
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

using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
using MauiForKimai.Messenger;
using MauiForKimai.ViewModels.Activity;
using MauiForKimai.ViewModels.Projects;
using MauiForKimai.Views.Timesheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MauiForKimai.ViewModels.Timesheets;

public partial class TimesheetCreateViewModel : ViewModelBase
{
    static Page Page => Application.Current?.MainPage ?? throw new NullReferenceException();
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
        WeakReferenceMessenger.Default.Register<TimesheetProjectChooseMessage>(this, (r, m) =>
        {
            ChosenProject = m.Value;
            Timesheet.Project = ChosenProject.Id;
            IsProjectNotValid = false;
        });

        WeakReferenceMessenger.Default.Register<TimesheetActivityChooseMessage>(this, (r, m) =>
        {
            ChosenActivity = m.Value;
            Timesheet.Activity = ChosenActivity.Id;
            IsActivityNotValid = false;
        });

        WeakReferenceMessenger.Default.Register<TimesheetCustomerChooseMessage>(this, (r, m) =>
        {
            ChosenCustomer = m.Value;
            // validation, that user never pick project, which customer do not contain
            if(ChosenProject != null) ChosenProject = null;
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
    CustomerListModel chosenCustomer;

    [ObservableProperty]
    ProjectListModel chosenProject;

    [ObservableProperty]
    ActivityListModel chosenActivity;

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
        var route = routingService.GetRouteByViewModel<ProjectChooseViewModel>();
        await Navigation.NavigateTo(route, ChosenCustomer);
    }

    [RelayCommand]
    async Task ShowActivityChooseView()
    {
        var route = routingService.GetRouteByViewModel<ActivityChooseViewModel>();
        await Navigation.NavigateTo(route, ChosenProject);
    }

    [RelayCommand]
    async Task ShowCustomerChooseView()
    {
        var route = routingService.GetRouteByViewModel<CustomerChooseViewModel>();
        await Navigation.NavigateTo(route);
    }

    [RelayCommand]
    async Task StartTimesheet()
    {
        var startTime = new DateTimeOffset(BeginDate.Year, BeginDate.Month, BeginDate.Day, BeginTime.Hours, BeginTime.Minutes, BeginTime.Seconds, loginService.GetUserTimeOffset());
        //var end = new DateTimeOffset(EndDate.Year, EndDate.Month, EndDate.Day, EndTime.Hours, EndTime.Minutes, EndTime.Seconds, new TimeSpan(0,0,0));
        Timesheet.Begin = startTime;
      

        if(!Validate()) return;

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

        WeakReferenceMessenger.Default.Send(new TimesheetStartMessage(Timesheet));
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

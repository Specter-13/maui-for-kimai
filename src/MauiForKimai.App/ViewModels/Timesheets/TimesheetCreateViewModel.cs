using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
using MauiForKimai.Messenger;
using MauiForKimai.Models;
using MauiForKimai.ViewModels.Activity;
using MauiForKimai.ViewModels.Projects;
using MauiForKimai.Views.Timesheets;
using Mopups.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MauiForKimai.ViewModels.Timesheets;

public partial class TimesheetCreateViewModel : ViewModelBase
{
    static Page Page => Application.Current?.MainPage ?? throw new NullReferenceException();
    private readonly IProjectService _projectService;

    public TimesheetCreateViewModel(IRoutingService rs,ILoginService ls, IProjectService projectService) : base(rs, ls)
    {
        _projectService = projectService;
         RegisterMessages();

         Timesheet = new TimesheetEditForm();
        Timesheet.Begin = new DateTimeOffset(DateTime.Now);

        BeginTime = Timesheet.Begin.TimeOfDay;
        //EndTime = Timesheet.End.Value.TimeOfDay;

        BeginDate = Timesheet.Begin.Date;
        //EndDate = Timesheet.End.Value.Date;

        Duration = (EndTime - BeginTime).ToString(@"hh\:mm\:ss");
        BeginDateString = BeginDate.ToString("dddd, dd MMMM yyyy");
    }

    private void RegisterMessages()
    { 
        WeakReferenceMessenger.Default.Register<TimesheetProjectChooseMessage>(this, (r, m) =>
        {
            ChosenProject = m.Value;
            Timesheet.Project = ChosenProject.Id;
        });

        WeakReferenceMessenger.Default.Register<TimesheetActivityChooseMessage>(this, (r, m) =>
        {
            ChosenActivity = m.Value;
            Timesheet.Activity = ChosenActivity.Id;
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
    ProjectListModel chosenProject;

    [ObservableProperty]
    ActivityListModel chosenActivity;

    [ObservableProperty]
    string selectedBillableMode;

    public override async Task OnParameterSet()
    {
        var x  = NavigationParameter as string;
        //Timesheet = new TimesheetEditForm();
        //Timesheet.Begin = new DateTimeOffset(DateTime.Now);

        //BeginTime = Timesheet.Begin.TimeOfDay;
        ////EndTime = Timesheet.End.Value.TimeOfDay;

        //BeginDate = Timesheet.Begin.Date;
        ////EndDate = Timesheet.End.Value.Date;

        //Duration = (EndTime - BeginTime).ToString(@"hh\:mm\:ss");
        //BeginDateString = BeginDate.ToString("dddd, dd MMMM yyyy");

    }


    [RelayCommand]
    async Task ShowProjectChooseView()
    {
        var route = routingService.GetRouteByViewModel<ProjectChooseViewModel>();
        await Navigation.NavigateTo(route);
    }

    [RelayCommand]
    async Task ShowActivityChooseView()
    {
        var route = routingService.GetRouteByViewModel<ActivityChooseViewModel>();
        await Navigation.NavigateTo(route);
    }

    [RelayCommand]
    async Task StartTimesheet()
    {
        var startTime = new DateTimeOffset(BeginDate.Year, BeginDate.Month, BeginDate.Day, BeginTime.Hours, BeginTime.Minutes, BeginTime.Seconds, new TimeSpan(1,0,0));
        //var end = new DateTimeOffset(EndDate.Year, EndDate.Month, EndDate.Day, EndTime.Hours, EndTime.Minutes, EndTime.Seconds, new TimeSpan(0,0,0));
        Timesheet.Begin = startTime;
        Timesheet.Billable = false;
        Timesheet.User = base.ApiStateProvider.ActualUser.Id;
        Timesheet.FixedRate = 0;
        Timesheet.HourlyRate = 0;
        //Timesheet.P

        WeakReferenceMessenger.Default.Send(new TimesheetStartMessage(Timesheet));
        //WeakReferenceMessenger.Default.Send
        await Navigation.NavigateTo("..");

    }



}

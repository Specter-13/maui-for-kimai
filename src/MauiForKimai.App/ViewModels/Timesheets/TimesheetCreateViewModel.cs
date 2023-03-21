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

    public TimesheetCreateViewModel(ApiStateProvider asp, IRoutingService routingService, IProjectService projectService) : base(asp, routingService)
    {
        _projectService = projectService;

        WeakReferenceMessenger.Default.Register<TimesheetProjectChooseMessage>(this, (r, m) =>
        {
            ActualProject = m.Value;
        });

         WeakReferenceMessenger.Default.Register<TimesheetActivityChooseMessage>(this, (r, m) =>
        {
            ActualActivity = m.Value;
        });
    }

    [ObservableProperty]
    TimesheetEditForm actualTimesheet;

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
    ProjectListModel actualProject;

    [ObservableProperty]
    ActivityListModel actualActivity;

    public override async Task OnParameterSet()
    {
        //ActualTimesheet = NavigationParameter as TimesheetEditForm;
        ActualTimesheet = new TimesheetEditForm();


        BeginTime = ActualTimesheet.Begin.TimeOfDay;
        EndTime = ActualTimesheet.End.Value.TimeOfDay;

        BeginDate = ActualTimesheet.Begin.Date;
        EndDate = ActualTimesheet.End.Value.Date;

        Duration = (EndTime - BeginTime).ToString(@"hh\:mm\:ss");
        BeginDateString = BeginDate.ToString("dddd, dd MMMM yyyy");

    }


    [RelayCommand]
    async Task ShowProjectMopup()
    {
        //var vm = new TimesheetProjectChooseMopupViewModel(_projectService);
        var route = RoutingService.GetRouteByViewModel<ProjectChooseViewModel>();
        await Navigation.NavigateTo(route);
     
    
    }

    [RelayCommand]
    async Task ShowActivityChooseView()
    {
        //var vm = new TimesheetProjectChooseMopupViewModel(_projectService);
        var route = RoutingService.GetRouteByViewModel<ActivityChooseViewModel>();
        await Navigation.NavigateTo(route);
     
    }

    [RelayCommand]
    async Task StartTimesheet()
    {
        ActualTimesheet.Project = ActualProject.Id;
        
        WeakReferenceMessenger.Default.Send(new TimesheetStartMessage(ActualTimesheet));
        //WeakReferenceMessenger.Default.Send
        await Navigation.NavigateTo("..");

    }



}

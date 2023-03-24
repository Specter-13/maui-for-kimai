using CommunityToolkit.Mvvm.Messaging;
using MauiForKimai.Messenger;
using MauiForKimai.Models;
using MauiForKimai.ViewModels.Timesheets;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ViewModels.Activity;

public partial class ActivityChooseViewModel : ViewModelBase
{

    private readonly IActivityService _activityService;
 
    
    public ActivityChooseViewModel(IRoutingService rs, ILoginService ls, IActivityService activityService) : base(rs,ls)
    {
        _activityService = activityService;
    }

    [ObservableProperty]
    ProjectListModel chosenProject;
    public async override Task OnParameterSet()
    {
        IsBusy = true;

        if (NavigationParameter is ProjectListModel project)
        {
            ChosenProject = project;
        }

        
        await GetActivities();
        
      

        IsBusy = false;
    }
    
    [ObservableProperty]
    ActivityListModel selectedActivity;


    private List<ActivityListModel> _allActivites {get; set;}= new();
    public ObservableCollection<ActivityListModel> SearchResults {get; set;} = new ObservableCollection<ActivityListModel>();


    [RelayCommand]
    void Filter(string filterText)
    {
        var filtered = _allActivites.Where(p => p.Name.Contains(filterText,StringComparison.InvariantCultureIgnoreCase));
        SearchResults.Clear();
        foreach(var activity in filtered)
        { 
            SearchResults.Add(activity);
        }
        
    }

    [ObservableProperty]
    bool isBusy;
    [ObservableProperty]
    bool showCollection;


    [RelayCommand]
    async Task ActivityTapped(ActivityListModel model)
    {
        SelectedActivity = model;
        WeakReferenceMessenger.Default.Send(new TimesheetActivityChooseMessage(SelectedActivity));
        var route = routingService.GetRouteByViewModel<TimesheetCreateViewModel>();
        await Navigation.NavigateTo("..");
    }
    

    public async Task GetActivities()
    { 
        ShowCollection = true;

        ICollection<ActivityCollection> activities;

        if(ChosenProject != null)
        { 
           activities = await _activityService.GetActivitiesByProject(ChosenProject.Id);  
        }
        else
        {
           activities = await _activityService.GetGlobalActivities(); 
        }
      
        
        foreach (var activity in activities) 
        {
            var activityListModel = new ActivityListModel((int)activity.Id, activity.Name);
            _allActivites.Add(activityListModel);
            SearchResults.Add(activityListModel);
        }

        ShowCollection = true;
    }
}

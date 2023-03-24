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

    public override async Task OnAppearing()
    {
        GetActivities();
		
	}
    
    [ObservableProperty]
    ActivityListModel selectedActivity;


    private List<ActivityListModel> _allActivites {get; set;}= new();
    public ObservableCollection<ActivityListModel> SearchResults {get; set;} = new ObservableCollection<ActivityListModel>();


    [RelayCommand]
    void FilterActivity(string filterText)
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
        IsBusy = true;
        var activities = await _activityService.GetActivities();  
        
        foreach (var activity in activities) 
        {
            var activityListModel = new ActivityListModel((int)activity.Id, activity.Name);
            _allActivites.Add(activityListModel);
            SearchResults.Add(activityListModel);
        }

        IsBusy = false;
        ShowCollection = true;
    }
}

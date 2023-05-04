using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Messaging;
using MauiForKimai.ApiClient;
using MauiForKimai.Core.Models;
using MauiForKimai.Messenger;
using MauiForKimai.Wrappers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ViewModels;
public partial class ChooseItemViewModel : ViewModelBase, IViewModelTransient
{
    public ChooseItemViewModel(IRoutingService rs, 
        ILoginService ls, 
        ICustomerService customerService, 
        IActivityService activityService,
        IProjectService projectService) : base(rs, ls)
    {
        _customerService = customerService;
        _activityService = activityService;
        _projectService = projectService;
    }


    private readonly IProjectService _projectService;
    private readonly ICustomerService _customerService;
    private readonly IActivityService _activityService;
 

   
    [ObservableProperty]
    string pageLabel;

    [ObservableProperty]
    IChooseItem selectedItem;
    public async override Task OnParameterSet()
    {
        IsBusy = true;

        if (NavigationParameter is ChooseItemWrapper wrapper)
        {

            if(base.HasInternetAndIsLogged())
            { 
                if(wrapper.ChooseItem is CustomerListModel)
                { 
                    PageLabel = "Select customer";
                    await GetCustomers();
                }

                if(wrapper.ChooseItem is ProjectListModel)
                { 
                    PageLabel = "Select Project";
                    await GetProjects(wrapper.ChosenCustomerId);
                }

                if(wrapper.ChooseItem is ActivityListModel)
                { 
                    PageLabel = "Select Activity";
                    await GetActivities(wrapper.ChosenProjectId);
                }
            }
            else
            { 
                await Toast.Make("Cannot acquire data! Check you internet connection.", ToastDuration.Short, 14).Show();
            }
        }

        IsBusy = false;
    }
  
    


    private List<IChooseItem> _allItems {get; set;}= new();
    public ObservableCollection<IChooseItem> SearchResults {get; set;} = new ObservableCollection<IChooseItem>();


    [RelayCommand]
    async Task Filter(string filterText)
    {
        if(base.HasInternetAndIsLogged())
        { 
            var filtered = _allItems.Where(p => p.Name.Contains(filterText,StringComparison.InvariantCultureIgnoreCase));
            SearchResults.Clear();
            foreach(var item in filtered)
            { 
                SearchResults.Add(item);
            }
        }
        else
        {
            await Toast.Make("Cannot acquire data!", ToastDuration.Short, 14).Show();
        }
        
    }



    [RelayCommand]
    async Task ItemTapped(IChooseItem item)
    {
        var wrapper = new ChooseItemWrapper(item);
        await Navigation.NavigateTo("..", wrapper);
    }
    

    private void AddToLists(IChooseItem item)
    { 
        _allItems.Add(item);
        SearchResults.Add(item);
    }

    public async Task GetCustomers()
    { 
        IsBusy = true;
        var customers = await _customerService.GetCustomers();  
        
        foreach (var customer in customers) 
        {
            var customerListModel = new CustomerListModel(customer.Id.Value, customer.Name, customer.Billable, customer.Color);
            AddToLists(customerListModel);
        }

        IsBusy = false;
    }

 

    public async Task GetProjects(int chosenCustomerId)
    {

        ICollection<ProjectCollection> projects;
        if (chosenCustomerId != 0)
        {
            projects = await _projectService.GetProjectsByCustomer(chosenCustomerId);
        }
        else
        {
            projects = await _projectService.GetProjects();
        }

        foreach (var project in projects)
        {
            var projectListModel = new ProjectListModel(project.Id.Value, project.Name, project.Customer.Value, project.Billable, project.Color);
            AddToLists(projectListModel);
        }

    }


    public async Task GetActivities(int chosenProjectId)
    { 

        ICollection<ActivityCollection> activities;

        if(chosenProjectId != 0)
        { 
           activities = await _activityService.GetActivitiesByProject(chosenProjectId);  
        }
        else
        {
           activities = await _activityService.GetGlobalActivities(); 
        }
      
        
        foreach (var activity in activities) 
        {
            var activityListModel = new ActivityListModel((int)activity.Id, activity.Name, activity.Billable, activity.Color);
            AddToLists(activityListModel);
        }

    }
}



public partial class ActivityChooseFavouriteViewModel : IViewModelTransient
{
}

public partial class ActivityChooseRecentTimesheetViewModel : IViewModelTransient
{
}

public partial class ActivityChooseDetailAllViewModel : IViewModelTransient
{
}


public partial class CustomerChooseFavouriteViewModel : IViewModelTransient
{
}

public partial class CustomerChooseRecentTimesheetViewModel: IViewModelTransient
{
}

public partial class CustomerChooseDetailAllViewModel : IViewModelTransient
{
}

public partial class ProjectChooseFavouriteViewModel: IViewModelTransient
{
}

public partial class ProjectChooseRecentTimesheetViewModel : IViewModelTransient
{
}

public partial class ProjectChooseDetailAllViewModel : IViewModelTransient
{
}


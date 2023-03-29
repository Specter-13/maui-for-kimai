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
public partial class ChooseItemViewModel : ViewModelBase
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
 

    private int _mode {get; set; }
   
    [ObservableProperty]
    string pageLabel;

    [ObservableProperty]
    IChooseItem selectedItem;
    public async override Task OnParameterSet()
    {
        IsBusy = true;

        if (NavigationParameter is ChooseItemWrapper wrapper)
        {
            _mode = (int)wrapper.Mode;
            if(wrapper.ChooseItem is CustomerListModel)
            { 
                //ChosenCustomer = customer;
                PageLabel = "Select customer";
                await GetCustomers();
            }

            if(wrapper.ChooseItem is ProjectListModel)
            { 
                //ChosenCustomer = customer;
                PageLabel = "Select Project";
                await GetProjects(wrapper.ChosenCustomerId);
            }

            if(wrapper.ChooseItem is ActivityListModel)
            { 
                //ChosenCustomer = customer;
                PageLabel = "Select Activity";
                await GetActivities(wrapper.ChosenProjectId);
                //await GetCustomers();
            }
        }

        IsBusy = false;
    }
  
    


    private List<IChooseItem> _allItems {get; set;}= new();
    public ObservableCollection<IChooseItem> SearchResults {get; set;} = new ObservableCollection<IChooseItem>();


    [RelayCommand]
    void Filter(string filterText)
    {
        var filtered = _allItems.Where(p => p.Name.Contains(filterText,StringComparison.InvariantCultureIgnoreCase));
        SearchResults.Clear();
        foreach(var item in filtered)
        { 
            SearchResults.Add(item);
        }
        
    }



    [RelayCommand]
    async Task ItemTapped(IChooseItem item)
    {
        //SelectedProject = model;

        var wrapper = new ChooseItemWrapper(item,ChooseItemMode.Favourite);
        WeakReferenceMessenger.Default.Send<ItemChooseMessage,int>(new ItemChooseMessage(wrapper), _mode);
        await Navigation.NavigateTo("..");
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
            var customerListModel = new CustomerListModel(customer.Id.Value, customer.Name, customer.Billable);
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
            var projectListModel = new ProjectListModel(project.Id.Value, project.Name, project.Customer.Value, project.Billable);
            AddToLists(projectListModel);
        }

    }


    public async Task GetActivities(int chosenProjectId)
    { 

        ICollection<ActivityCollection> activities;

        if(chosenProjectId != null)
        { 
           activities = await _activityService.GetActivitiesByProject(chosenProjectId);  
        }
        else
        {
           activities = await _activityService.GetGlobalActivities(); 
        }
      
        
        foreach (var activity in activities) 
        {
            var activityListModel = new ActivityListModel((int)activity.Id, activity.Name, activity.Billable);
            AddToLists(activityListModel);
        }

    }
}



public partial class ActivityChooseFavouriteViewModel : ChooseItemViewModel
{
    public ActivityChooseFavouriteViewModel(IRoutingService rs, ILoginService ls, ICustomerService customerService, IActivityService activityService, IProjectService projectService) : base(rs, ls, customerService, activityService, projectService)
    {
    }
}

public partial class ActivityChooseTimesheetViewModel : ChooseItemViewModel
{
    public ActivityChooseTimesheetViewModel(IRoutingService rs, ILoginService ls, ICustomerService customerService, IActivityService activityService, IProjectService projectService) : base(rs, ls, customerService, activityService, projectService)
    {
    }
}

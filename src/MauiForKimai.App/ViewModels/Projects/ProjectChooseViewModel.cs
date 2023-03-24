using CommunityToolkit.Mvvm.Messaging;
using MauiForKimai.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiForKimai.Messenger;
using MauiForKimai.ApiClient.Services;
using MauiForKimai.ViewModels.Timesheets;

namespace MauiForKimai.ViewModels.Projects;

public partial class ProjectChooseViewModel : ViewModelBase
{
     private readonly IProjectService _projectService;
 
    
    public ProjectChooseViewModel(IRoutingService rs,ILoginService ls,IProjectService projectService) : base(rs, ls)
    {
        _projectService = projectService;
    }

    [ObservableProperty]
    CustomerListModel chosenCustomer;

    public async override Task OnParameterSet()
    {
        IsBusy = true;

        if (NavigationParameter is CustomerListModel customer)
        {
            ChosenCustomer = customer;
        }

       
        await GetProjects();
        
      

        IsBusy = false;
    }
  
    
    [ObservableProperty]
    ProjectListModel selectedProject;


    private List<ProjectListModel> _allProjects {get; set;}= new();
    public ObservableCollection<ProjectListModel> SearchResults {get; set;} = new ObservableCollection<ProjectListModel>();


    [RelayCommand]
    void Filter(string filterText)
    {
        var filtered = _allProjects.Where(p => p.Name.Contains(filterText,StringComparison.InvariantCultureIgnoreCase));
        SearchResults.Clear();
        foreach(var project in filtered)
        { 
            SearchResults.Add(project);
        }
        
    }

    [ObservableProperty]
    bool isBusy;
    [ObservableProperty]
    bool showCollection;


    [RelayCommand]
    async Task ProjectTapped(ProjectListModel model)
    {
        SelectedProject = model;
        WeakReferenceMessenger.Default.Send(new TimesheetProjectChooseMessage(SelectedProject));
        var route = routingService.GetRouteByViewModel<TimesheetCreateViewModel>();
        await Navigation.NavigateTo("..");
    }
    

    public async Task GetProjects()
    { 
        ShowCollection = true;

        ICollection<ProjectCollection> projects;
        if(ChosenCustomer != null)
        { 
            projects = await _projectService.GetProjectsByCustomer(ChosenCustomer.Id);  
        }
        else
        { 
            projects = await _projectService.GetProjects();  
        }
        
        foreach (var project in projects) 
        {
            var projectListModel = new ProjectListModel((int)project.Id, project.Name);
            _allProjects.Add(projectListModel);
            SearchResults.Add(projectListModel);
        }

        ShowCollection = true;
    }
}

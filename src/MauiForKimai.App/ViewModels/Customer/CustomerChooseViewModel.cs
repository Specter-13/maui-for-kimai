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

namespace MauiForKimai.ViewModels;
public partial class CustomerChooseViewModel : ViewModelBase
{
    private readonly ICustomerService _customerService;
    public CustomerChooseViewModel(IRoutingService rs, ILoginService ls, ICustomerService customerService) : base(rs, ls)
    {
        _customerService = customerService;
    }


    public override async Task Initialize()
    {
        await GetCustomers();
	}
    
    [ObservableProperty]
    CustomerListModel selectedCustomer;


    private List<CustomerListModel> _allCustomers {get; set;}= new();
    public ObservableCollection<CustomerListModel> SearchResults {get; set;} = new ObservableCollection<CustomerListModel>();


    [RelayCommand]
    void Filter(string filterText)
    {
        var filtered = _allCustomers.Where(p => p.Name.Contains(filterText,StringComparison.InvariantCultureIgnoreCase));
        SearchResults.Clear();
        foreach(var customer in filtered)
        { 
            SearchResults.Add(customer);
        }
        
    }

    [ObservableProperty]
    bool isBusy;
    [ObservableProperty]
    bool showCollection;


    [RelayCommand]
    async Task CustomerTapped(CustomerListModel model)
    {
        SelectedCustomer = model;
        WeakReferenceMessenger.Default.Send(new TimesheetCustomerChooseMessage(SelectedCustomer));
        //var route = routingService.GetRouteByViewModel<TimesheetCreateViewModel>();
        await Navigation.NavigateTo("..");
    }
    

    public async Task GetCustomers()
    { 
        ShowCollection = true;
        IsBusy = true;
        var customers = await _customerService.GetCustomers();  
        
        foreach (var customer in customers) 
        {
            var customerListModel = new CustomerListModel(customer.Id.Value, customer.Name);
            _allCustomers.Add(customerListModel);
            SearchResults.Add(customerListModel);
        }

        IsBusy = false;
        ShowCollection = true;
    }
}

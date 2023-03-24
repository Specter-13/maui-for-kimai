using CommunityToolkit.Mvvm.Input;
using MauiForKimai.ViewModels.Base;
using MauiForKimai.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ViewModels;

public partial class MenuViewModel : ViewModelBase
{
    public MenuViewModel(IRoutingService rs, ILoginService ls) : base(rs, ls)
    {
    }

    [RelayCommand]
    async Task GoToHomeAsync()
    {
        if(base.ApiStateProvider.IsAuthenticated)
        { 
            var route = base.routingService.GetRouteByViewModel<HomeViewModel>();
            await Navigation.NavigateTo(route);
        }
    }

     [RelayCommand]
    async Task GoToLoginAsync()
    {
         var route = base.routingService.GetRouteByViewModel<ServerListViewModel>();
        await Navigation.NavigateTo(route);
    }

    [RelayCommand]
    async Task LogOutAsync()
    { 
        await GoToLoginAsync();
		    base.ApiStateProvider.Disconnect();
        
		    //await Shell.Current.GoToAsync(nameof(LoginView));
	    }

}

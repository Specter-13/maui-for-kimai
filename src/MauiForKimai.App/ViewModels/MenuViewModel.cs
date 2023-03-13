using CommunityToolkit.Mvvm.Input;
using MauiForKimai.ApiClient.ApiClient;
using MauiForKimai.ViewModels.Base;
using MauiForKimai.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ViewModels
{
    public partial class MenuViewModel : ViewModelBase
    {
        public MenuViewModel(ApiStateProvider asp, IRoutingService routingService) : base(asp, routingService)
        {
        }

        [RelayCommand]
        async Task GoToHomeAsync()
        {
            if(base.ApiStateProvider.IsAuthenticated)
            { 
                var route = base.RoutingService.GetRouteByViewModel<MainViewModel>();
                await Shell.Current.GoToAsync(route);
            }
        }

         [RelayCommand]
        async Task GoToLoginAsync()
        {
             var route = base.RoutingService.GetRouteByViewModel<LoginViewModel>();
            await Shell.Current.GoToAsync(route);
        }

        [RelayCommand]
        async Task LogOutAsync()
        { 
		    base.ApiStateProvider.Disconnect();
		    //await Shell.Current.GoToAsync(nameof(LoginView));
	    }

    }
}

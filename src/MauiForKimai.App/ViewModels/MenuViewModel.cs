using CommunityToolkit.Mvvm.Input;
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
                var route = base.RoutingService.GetRouteByViewModel<HomeViewModel>();
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
            await GoToLoginAsync();
		    //await Shell.Current.GoToAsync(nameof(LoginView));
	    }

    }
}

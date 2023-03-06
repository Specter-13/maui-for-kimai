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
        public MenuViewModel(ApiStateProvider asp) : base(asp)
        {
        }

        [RelayCommand]
        async Task GoToHomeAsync()
        {
            await Shell.Current.GoToAsync("//homeview");
        }

         [RelayCommand]
        async Task GoToLoginAsync()
        {
            await Shell.Current.GoToAsync("//loginview");
        }

        [RelayCommand]
        async Task LogOutAsync()
        { 
		    base.ApiStateProvider.Disconnect();
		    //await Shell.Current.GoToAsync(nameof(LoginView));
	    }

    }
}

using CommunityToolkit.Mvvm.Input;
using MauiForKimai.ApiClient.Authentication;
using MauiForKimai.Pages.ServersManagement;
using MauiForKimai.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ViewModels;
public partial class LoginViewModel : ViewModelBase
{
    public LoginViewModel(AuthHandler aut) : base(aut)
    {

    }
    [RelayCommand]
    async Task AddNewServer() 
    {
        await Shell.Current.GoToAsync(nameof(ServerDetailPage));
    }

}

using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiForKimai.ApiClient.Authentication;
using MauiForKimai.Models;
using MauiForKimai.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ViewModels;
public partial class ServerDetailViewModel : ViewModelBase
{
	
    [ObservableProperty]
	private string name;
    [ObservableProperty]
	private string url;
    [ObservableProperty]
	private string username;
	[ObservableProperty]
	private string apiPassword;
	[ObservableProperty]
	private string isDefault;


    [ObservableProperty]
    private ServerModel server = new();

    public ServerDetailViewModel(IRoutingService rs, ILoginService ls) : base(rs, ls)
    {
    }


    public override Task OnParameterSet()
    {
        IsBusy = true;

        if (NavigationParameter is ServerModel myServer)
        {
            Server = myServer;
        }
      

        IsBusy = false;
        return base.OnParameterSet();
    }

    [RelayCommand]
    async Task Connect() 
    {

        IToast toast;
        if (loginService.CheckIfConnected(Server))
        {
            toast = Toast.Make($"Already connected to {Server.Name}!", ToastDuration.Short, 14);
            await toast.Show();
            return;
        }



        var isSuccess = await loginService.Login(Server);
        
        if(isSuccess) 
        {
            toast = Toast.Make("Connection to Kimai established!", ToastDuration.Short, 14);
        }
		else
		{
			toast = Toast.Make("Connection to Kimai failed! Check your credentials!", ToastDuration.Short, 14);
		}
        await toast.Show();
    }

    [RelayCommand]
    async Task TestConnection() 
    {

		var isSuccess = await loginService.TestConnection(Server);
        IToast toast;
        if(isSuccess) 
        {
            toast = Toast.Make("Test successfull! Server is reachable!", ToastDuration.Short, 14);
        }
		else
		{
			toast = Toast.Make("Connection to Kimai failed! Check your url and internet connection!", ToastDuration.Short, 14);
		}
        await toast.Show();
    }

    [RelayCommand]
    async Task Disconnect() 
    {

		await loginService.Logout();
        await Toast.Make("Disconected successfully!", ToastDuration.Short, 14).Show();

    }

    //[RelayCommand]
    //   async Task Add()
    //   { 

    //}
}

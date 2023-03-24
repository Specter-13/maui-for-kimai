using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using MauiForKimai.ApiClient.Authentication;
using MauiForKimai.ViewModels;
using MauiForKimai.ViewModels.Base;
using MauiForKimai.Views;

namespace MauiForKimai.Shells;

public partial class AppShellMobile : Shell
{
	public AppShellMobile(MenuViewModel menuViewModel)
	{
		InitializeComponent();
		BindingContext = menuViewModel;

	}

	//protected override async Task OnInitialize()
	//{
	//	var ls = MauiForKimai.DependencyInjection.ServiceProvider.GetService<ILoginService>();
	//	var status = await ls.LoginToDefaultOnStartUp();

	//	var toast = Toast.Make("Connection to Kimai established!", ToastDuration.Short, 14);
	//	await toast.Show();


	//}
}

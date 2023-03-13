using MauiForKimai.ApiClient.Authentication;
using MauiForKimai.Pages.ServersManagement;
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
}

using MauiForKimai.Pages.ServersManagement;
using MauiForKimai.ViewModels;
using MauiForKimai.Views;

namespace MauiForKimai.Shells;

public partial class AppShellDesktop : Shell
{
	public AppShellDesktop(MenuViewModel menuViewModel) 
	{
		InitializeComponent();
		BindingContext = menuViewModel;
		//Routing.RegisterRoute(nameof(ServerDetailPage), typeof(ServerDetailPage));

		Routing.RegisterRoute("//loginview", typeof(LoginView));
		Routing.RegisterRoute("//homeview", typeof(HomeView));
	}
}
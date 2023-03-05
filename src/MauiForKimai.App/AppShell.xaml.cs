using MauiForKimai.ApiClient.Authentication;
using MauiForKimai.Pages.ServersManagement;
using MauiForKimai.ViewModels.Base;
using MauiForKimai.Views;

namespace MauiForKimai;

public partial class AppShell : Shell
{
	public AppShell(ViewModelBase baseViewModel)
	{
		InitializeComponent();
		BindingContext = baseViewModel;
		Routing.RegisterRoute(nameof(ServerDetailPage), typeof(ServerDetailPage));
		Routing.RegisterRoute(nameof(LoginView), typeof(LoginView));
	}
}

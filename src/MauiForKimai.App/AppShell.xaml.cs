using MauiForKimai.Pages.ServersManagement;

namespace MauiForKimai;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(ServerDetailPage), typeof(ServerDetailPage));
	}
}

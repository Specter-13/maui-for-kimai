using MauiForKimai.ViewModels;

namespace MauiForKimai.Pages.ServersManagement;

public partial class ServerDetailPage 
{
	public ServerDetailPage(ServerDetailViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}
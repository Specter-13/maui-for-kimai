using MauiForKimai.ViewModels;

namespace MauiForKimai.Views;

public partial class ServerListView
{
	public ServerListView(ServerListViewModel vm)
	{
		InitializeComponent();
		BindingContext= vm;
	}

}
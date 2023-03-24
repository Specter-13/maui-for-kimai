using MauiForKimai.ViewModels;

namespace MauiForKimai.Views;

public partial class ServerDetailView
{
	public ServerDetailView(ServerDetailViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}
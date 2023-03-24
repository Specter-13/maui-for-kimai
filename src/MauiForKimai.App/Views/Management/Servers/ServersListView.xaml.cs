using MauiForKimai.ViewModels;

namespace MauiForKimai.Views;

public partial class ServerListView
{
	public ServerListView(ServerListViewModel vm)
	{
		InitializeComponent();
		BindingContext= vm;
	}

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
		var x =10;
    }
}
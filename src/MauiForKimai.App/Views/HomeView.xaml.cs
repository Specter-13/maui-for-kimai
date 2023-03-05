using MauiForKimai.ViewModels;

namespace MauiForKimai.Views;

public partial class HomeView : ContentPage
{
	private readonly MainViewModel _vm;
	public HomeView(MainViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    protected override void OnAppearing()
    {
		//_vm.
        base.OnAppearing();	
    }
}
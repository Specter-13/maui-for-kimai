using MauiForKimai.Helpers;
using MauiForKimai.ViewModels;

namespace MauiForKimai;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage(MainViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}


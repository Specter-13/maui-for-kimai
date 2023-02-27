using MauiForKimai.Helpers;
using MauiForKimai.ViewModels;

namespace MauiForKimai.Pages;

public partial class MainPage : ContentPage
{

	public MainPage(MainViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}


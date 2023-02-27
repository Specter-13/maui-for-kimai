using MauiForKimai.ViewModels;

namespace MauiForKimai.Pages;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginViewModel vm)
	{
		InitializeComponent();
		BindingContext= vm;
	}
}
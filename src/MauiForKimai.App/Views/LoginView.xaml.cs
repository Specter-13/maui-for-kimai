using MauiForKimai.ViewModels;

namespace MauiForKimai.Views;

public partial class LoginView : ContentPage
{
	public LoginView(LoginViewModel vm)
	{
		InitializeComponent();
		BindingContext= vm;
	}
}
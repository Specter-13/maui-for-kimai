using MauiForKimai.ViewModels;

namespace MauiForKimai.Views;

public partial class LoginView
{
	public LoginView(LoginViewModel vm)
	{
		InitializeComponent();
		BindingContext= vm;
	}
}
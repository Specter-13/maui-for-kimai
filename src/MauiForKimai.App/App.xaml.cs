

using MauiForKimai.ApiClient.Authentication;
using MauiForKimai.ViewModels.Base;

namespace MauiForKimai;

public partial class App : Application
{
	public App(ViewModelBase baseViewModel)
	{
		InitializeComponent();

		MainPage = new AppShell(baseViewModel);
	}
}

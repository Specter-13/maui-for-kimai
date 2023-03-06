

using MauiForKimai.ApiClient.Authentication;
using MauiForKimai.Shells;
using MauiForKimai.ViewModels;
using MauiForKimai.ViewModels.Base;
using Microsoft.Extensions.DependencyInjection;

namespace MauiForKimai;

public partial class App : Application
{
	public App(MenuViewModel menuViewModel)
	{
		this.UserAppTheme = Microsoft.Maui.ApplicationModel.AppTheme.Light;
		InitializeComponent();


		if (DeviceInfo.Idiom == DeviceIdiom.Phone)
        {
            MainPage = new AppShellMobile(menuViewModel);
        }
        else
        {
            MainPage = new AppShellDesktop(menuViewModel);
        }
		
		//MainPage = new AppShell(menuViewModel);
	}
}

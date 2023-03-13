

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
		InitializeComponent();


		#if ANDROID
            MainPage = new AppShellMobile(menuViewModel);
		#else
            MainPage = new AppShellDesktop(menuViewModel);
		#endif
        
		
		//MainPage = new AppShell(menuViewModel);
	}
}

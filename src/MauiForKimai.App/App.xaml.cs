

using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using MauiForKimai.ApiClient.Authentication;
using MauiForKimai.Shells;
using MauiForKimai.ViewModels;
using MauiForKimai.ViewModels.Base;
using Microsoft.Extensions.DependencyInjection;
using TinyMvvm;

namespace MauiForKimai;

public partial class App : TinyApplication
{
	private readonly ILoginService _loginService;
	public App(MenuViewModel menuViewModel, ILoginService loginService)
	{

		InitializeComponent();
		_loginService = loginService;




		#if ANDROID
            MainPage = new AppShellMobile(menuViewModel);
		#else
            MainPage = new AppShellDesktop(menuViewModel);
		#endif
        

		//try to login on startup to default application
		
		//MainPage = new AppShell(menuViewModel);
	}

	
}

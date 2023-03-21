

using MauiForKimai.ApiClient.Authentication;
using MauiForKimai.Shells;
using MauiForKimai.ViewModels;
using MauiForKimai.ViewModels.Base;
using Microsoft.Extensions.DependencyInjection;
using TinyMvvm;

namespace MauiForKimai;

public partial class App : TinyApplication
{
	//private readonly ILoginService _loginService;
	public App(MenuViewModel menuViewModel, ILoginService loginService)
	{
		//_loginService = loginService;
		InitializeComponent();


		#if ANDROID
            MainPage = new AppShellMobile(menuViewModel);
		#else
            MainPage = new AppShellDesktop(menuViewModel);
		#endif
        
		//try to login on startup to default application
		
		//MainPage = new AppShell(menuViewModel);
	}

	//protected override async Task Initialize()
 //   {
 //       await base.Initialize();
		
		

	//	//To test that it not hangs the application.
	//	for(int i = 0; i < 100; i++)
	//	{
	//		await Task.Delay(1000);
	//	}
 //   }
}

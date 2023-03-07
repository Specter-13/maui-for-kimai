
using Microsoft.Extensions.Logging;
using MauiForKimai.ViewModels;
using CommunityToolkit.Maui;
using MauiForKimai.ApiClient.Authentication;
using Microsoft.Extensions.DependencyInjection;
using MauiForKimai.ApiClient.Interfaces;
using MauiForKimai.ApiClient.Services;
using MauiForKimai.Services.ServicesConfiguration;
using MauiForKimai.Pages;
using MauiForKimai.Pages.ServersManagement;
using MauiForKimai.ApiClient.Services.Configuration;
using MauiForKimai.Views;
using MauiForKimai.ViewModels.Base;
using MauiForKimai.Shells;

namespace MauiForKimai;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		//builder.Services.ConfigureShell();

		builder.Services.RegisterClientServices();
		builder.Services.RegisterAppServices();

		//ConfigureApiClients(builder.Services);
		builder.Services.AddSingleton<MenuViewModel>();
		builder.Services.AddSingleton<MainViewModel>();
		builder.Services.AddSingleton<LoginViewModel>();
		builder.Services.AddSingleton<TimeSheetViewModel>();

		builder.Services.AddTransient<ServerDetailViewModel>();
		builder.Services.AddTransient<ServerDetailPage>();


		builder.Services.AddSingleton<HomeView>();
		builder.Services.AddSingleton<LoginView>();
		builder.Services.AddSingleton<TimeSheetView>();

		
#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}

	public static void ConfigureShell(this IServiceCollection services)
    {
        if (DeviceInfo.Idiom == DeviceIdiom.Phone)
        {
            services.AddSingleton<AppShellMobile>();
        }
        else
        {
            services.AddSingleton<AppShellDesktop>();
        }
    }
}


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

		

		builder.Services.RegisterClientServices();
		builder.Services.RegisterAppServices();

		//ConfigureApiClients(builder.Services);
		builder.Services.AddSingleton<ViewModelBase>();
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

	// private static void ConfigureApiClients(IServiceCollection services)
	//{
	//	services.AddHttpClient<IUserClient, UserClient>((serviceProvider, client) =>
	//	{
	//		//var apiOptions = serviceProvider.GetRequiredService<IOptions<ApiOptions>>();
			
			
	//		client.BaseAddress = new Uri("https://specter13maui.kimai.cloud/");
	//		//client.BaseAddress = new Uri("https://demo-plugins.kimai.org/");
	//		//client.BaseAddress = new Uri("http://localhost:8001/");
	//		//client.DefaultRequestHeaders.Add("X-AUTH-USER", "admin@admin.com");
	//		client.DefaultRequestHeaders.Add("X-AUTH-USER", "dadkos34@gmail.com");
	//		client.DefaultRequestHeaders.Add("X-AUTH-TOKEN", "internet");
		
 //       });

       
 //   }
}

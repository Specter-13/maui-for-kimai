
using Microsoft.Extensions.Logging;
using MauiForKimai.ViewModels;
using CommunityToolkit.Maui;
using MauiForKimai.ApiClient.Authentication;
using Microsoft.Extensions.DependencyInjection;
using MauiForKimai.ApiClient.Interfaces;
using MauiForKimai.ApiClient.Services;
using MauiForKimai.BL.Services;

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
		

		//ConfigureApiClients(builder.Services);
		builder.Services.AddSingleton<MainViewModel>();
		builder.Services.AddSingleton<MainPage>();

		
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

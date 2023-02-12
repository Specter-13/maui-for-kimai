using MauiForKimai.BL.ApiClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using MauiForKimai.ViewModels;

namespace MauiForKimai;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		ConfigureApiClients(builder.Services);
		builder.Services.AddSingleton<MainViewModel>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}

	 private static void ConfigureApiClients(IServiceCollection services)
	{
		services.AddHttpClient<IUserClient, UserClient>((serviceProvider, client) =>
		{
			//var apiOptions = serviceProvider.GetRequiredService<IOptions<ApiOptions>>();
			
			
			client.BaseAddress = new Uri("https://demo-plugins.kimai.org/");
			//client.BaseAddress = new Uri("http://localhost:8001/");
			//client.DefaultRequestHeaders.Add("X-AUTH-USER", "admin@admin.com");
			client.DefaultRequestHeaders.Add("X-AUTH-USER", "susan_super@example.com");
			client.DefaultRequestHeaders.Add("X-AUTH-TOKEN", "password");
		
        });

       
    }
}

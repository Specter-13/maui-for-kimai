
using Microsoft.Extensions.Logging;
using MauiForKimai.ViewModels;
using CommunityToolkit.Maui;
using MauiForKimai.ApiClient.Authentication;
using Microsoft.Extensions.DependencyInjection;
using MauiForKimai.ApiClient.Interfaces;
using MauiForKimai.ApiClient.Services;
using MauiForKimai.Views;
using MauiForKimai.ViewModels.Base;
using MauiForKimai.Shells;
using MauiForKimai.Services;
using MauiForKimai.DependencyInjection;
using TinyMvvm;
using Mopups.Hosting;
using Mopups.Interfaces;
using Mopups.Services;
using MauiForKimai.ApiClient.DependencyInjection;

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
				fonts.AddFont(filename: "materialdesignicons-webfont.ttf", alias: "MaterialDesignIcons");
			})
			.UseTinyMvvm();



		builder.Services.RegisterClientServices();
		builder.Services.RegisterAppServices();


		builder.Services.ConfigureViewModels();
		builder.Services.ConfigureViews();

		
#if DEBUG
		builder.Logging.AddDebug();
#endif

		 var app = builder.Build();

        RegisterRoutes(app);

        return app;
	}


	private static void RegisterRoutes(MauiApp app)
    {
        var routingService = app.Services.GetRequiredService<IRoutingService>();

        foreach (var routeModel in routingService.Routes)
        {
            Routing.RegisterRoute(routeModel.Route, routeModel.ViewType);
        }
    }
}

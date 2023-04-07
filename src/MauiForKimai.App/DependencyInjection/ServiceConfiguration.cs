using MauiForKimai.ApiClient.Authentication;
using MauiForKimai.ApiClient.Interfaces;
using MauiForKimai.ApiClient.Services;
using MauiForKimai.Interfaces;
using MauiForKimai.Persistence;
using MauiForKimai.Services;
using MauiForKimai.Wrappers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyMvvm;

namespace MauiForKimai.DependencyInjection;

public static class ServicesConfiguration
{
    public static void RegisterAppServices(this IServiceCollection services)
    {
        
        services.AddSingleton<IRoutingService,RoutingService>();
        services.AddSingleton<ISecureStorageService,SecureStorageService>();
        services.AddSingleton<IServerService,ServerService>();
        services.AddSingleton<ILoginService,LoginService>();
        services.AddSingleton<IDispatcherWrapper,DispatcherWrapper>();
        services.AddSingleton<IFavouritesTimesheetService,FavouriteTimesheetService>();
    }

    public static void ConfigureViewModels(this IServiceCollection services)
    {
        services.Scan(selector => selector
            .FromAssemblyOf<App>()
            .AddClasses(filter => filter.AssignableTo<IViewModel>())
            .AsSelfWithInterfaces()
            .WithTransientLifetime());

        //i need reports view model as singleton as i share it between mulitple views
        services.AddSingleton<ReportsViewModel>();
    }

    public static void ConfigureViews(this IServiceCollection services)
    {
        services.Scan(selector => selector
            .FromAssemblyOf<App>()
            .AddClasses(filter => filter.AssignableTo<TinyView>())
            .AsSelfWithInterfaces()
            .WithTransientLifetime());
    }
}


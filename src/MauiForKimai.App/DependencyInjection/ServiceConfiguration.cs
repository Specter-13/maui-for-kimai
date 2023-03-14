using MauiForKimai.ApiClient.Authentication;
using MauiForKimai.ApiClient.Interfaces;
using MauiForKimai.ApiClient.Services;
using MauiForKimai.Interfaces;
using MauiForKimai.Services;
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
		services.AddSingleton<ServerService>();
        services.AddSingleton<ISecureStorageService,SecureStorageService>();
    }

    public static void ConfigureViewModels(this IServiceCollection services)
    {
        services.Scan(selector => selector
            .FromAssemblyOf<App>()
            .AddClasses(filter => filter.AssignableTo<IViewModel>())
            .AsSelfWithInterfaces()
            .WithTransientLifetime());
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


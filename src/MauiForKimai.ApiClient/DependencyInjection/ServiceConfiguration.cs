using MauiForKimai.ApiClient.Authentication;
using MauiForKimai.ApiClient.Interfaces;
using MauiForKimai.ApiClient.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ApiClient.DependencyInjection;

public static class ServicesConfiguration
{
    public static void RegisterClientServices(this IServiceCollection services)
    {
        services.AddSingleton<ApiLoginContext>();
        services.AddTransient<AuthHandler>();

        services.AddHttpClient(AuthHandler.AUTHENTICATED_CLIENT)
            .AddHttpMessageHandler((s) => s.GetService<AuthHandler>());

        

        services.AddSingleton<ApiClientWrapper>();
        services.ConfigureServices();

    }

    public static void ConfigureServices(this IServiceCollection services)
    {
        services.Scan(selector => selector
            .FromCallingAssembly()     
            .AddClasses(filter => filter.AssignableTo<IBaseService>())
            .AsSelfWithInterfaces()
            .WithSingletonLifetime());

    }
    //public static void AddSingleton<I1, I2, T>(this IServiceCollection services)
    //       where T : class, I1, I2
    //       where I1 : class
    //       where I2 : class
    //{
    //    services.AddSingleton<I1, T>();
    //    services.AddSingleton<I2, T>(x => (T)x.GetService<I1>());
    //}
}


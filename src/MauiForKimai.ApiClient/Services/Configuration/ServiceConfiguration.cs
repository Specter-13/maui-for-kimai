﻿using MauiForKimai.ApiClient.Authentication;
using MauiForKimai.ApiClient.Interfaces;
using MauiForKimai.ApiClient.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ApiClient.Services.Configuration;

public static class ServicesConfiguration
{
    public static void RegisterClientServices(this IServiceCollection services)
    {
        services.AddSingleton<ApiStateProvider>();
        services.AddTransient<AuthHandler>();

        services.AddHttpClient(AuthHandler.AUTHENTICATED_CLIENT)
            .AddHttpMessageHandler((s) => s.GetService<AuthHandler>());


        services.AddSingleton<IUserService,IBaseService,UserService>();
        services.AddSingleton<ITimesheetService,IBaseService,TimesheetService>();
        services.AddSingleton<IProjectService,IBaseService,ProjectService>();
        services.AddSingleton<IActivityService,IBaseService,ActivityService>();

    }

     public static void AddSingleton<I1, I2, T>(this IServiceCollection services) 
            where T : class, I1, I2
            where I1 : class
            where I2 : class
    {
        services.AddSingleton<I1, T>();
        services.AddSingleton<I2, T>(x => (T) x.GetService<I1>());
    }
}


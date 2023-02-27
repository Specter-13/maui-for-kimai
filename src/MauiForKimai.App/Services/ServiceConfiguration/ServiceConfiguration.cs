using MauiForKimai.ApiClient.Authentication;
using MauiForKimai.ApiClient.Interfaces;
using MauiForKimai.ApiClient.Services;
using MauiForKimai.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Services.ServicesConfiguration;

public static class ServicesConfiguration
{
    public static void RegisterAppServices(this IServiceCollection services)
    {
     
		services.AddSingleton<ServerService>();
    }
}


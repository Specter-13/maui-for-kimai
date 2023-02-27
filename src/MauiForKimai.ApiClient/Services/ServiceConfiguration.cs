using MauiForKimai.ApiClient.Authentication;
using MauiForKimai.ApiClient.Interfaces;
using MauiForKimai.ApiClient.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.BL.Services;

public static class ServicesConfiguration
{
    public static void RegisterClientServices(this IServiceCollection services)
    {
        services.AddSingleton<AuthHandler>();

        services.AddHttpClient(AuthHandler.AUTHENTICATED_CLIENT)
			.AddHttpMessageHandler((s) => s.GetService<AuthHandler>());

     
		services.AddSingleton<IUserService,UserService>();
    }
}


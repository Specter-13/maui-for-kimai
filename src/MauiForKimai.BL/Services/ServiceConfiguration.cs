using MauiForKimai.BL.Facades;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.BL.Services;

public static class ServicesConfiguration
{
    public static void AddFacades(this IServiceCollection services)
    {
        services.AddSingleton<UserFacade>();
    }
}


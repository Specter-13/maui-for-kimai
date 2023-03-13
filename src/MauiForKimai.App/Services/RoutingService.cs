using MauiForKimai.Models;
using MauiForKimai.ViewModels;
using MauiForKimai.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Services
{
    public class RoutingService : IRoutingService
    {
         private static ICollection<RouteModel> routesCommon = new List<RouteModel>
    {
  //           Routing.RegisterRoute("//loginview", typeof(LoginView));
		//Routing.RegisterRoute("//homeview", typeof(HomeView));
        new("//login", typeof(LoginView), typeof(LoginViewModel)),
        new("//home", typeof(HomeView), typeof(HomeViewModel)),
        new("//timesheets", typeof(TimeSheetView), typeof(TimeSheetViewModel)),
    };

    private static IEnumerable<RouteModel> routesPhone = new List<RouteModel>
    {
  
    }.Concat(routesCommon);

    private static IEnumerable<RouteModel> routesDesktop = new List<RouteModel>
    {
       
    }.Concat(routesCommon);

         public IEnumerable<RouteModel> Routes
        => DeviceInfo.Idiom == DeviceIdiom.Phone
            ? routesPhone
            : routesDesktop;

        public string GetRouteByViewModel<TViewModel>() where TViewModel : IViewModel
        => Routes.First(route => route.ViewModelType == typeof(TViewModel)).Route;
    }
}

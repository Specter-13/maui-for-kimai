
using MauiForKimai.ViewModels;
using MauiForKimai.ViewModels.Activity;
using MauiForKimai.ViewModels.Timesheets;
using MauiForKimai.Views;
using MauiForKimai.Views.Activity;
using MauiForKimai.Views.Timesheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Services;

public class RoutingService : IRoutingService
{
     private static ICollection<RouteModel> routesCommon = new List<RouteModel>
{
//           Routing.RegisterRoute("//loginview", typeof(LoginView));
		//Routing.RegisterRoute("//homeview", typeof(HomeView));
    //new("//login", typeof(LoginView), typeof(LoginViewModel)),
    new("//home", typeof(HomeView), typeof(HomeViewModel)),
    new("//templates", typeof(TemplateListView), typeof(TemplateListViewModel)),

    new("//favourites", typeof(TimesheetFavouritesListView), typeof(TimesheetFavouritesListViewModel)),
    new("//favourites/create", typeof(TimesheetFavouritesCreateView), typeof(TimesheetFavouritesCreateViewModel)),
    new("//favourites/create/choosecustomer", typeof(ChooseItemView), typeof(CustomerChooseFavouriteViewModel)),
    new("//favourites/create/chooseproject", typeof(ChooseItemView), typeof(ProjectChooseFavouriteViewModel)),
    new("//favourites/create/chooseactivity", typeof(ChooseItemView), typeof(ActivityChooseFavouriteViewModel)),

    new("//reports", typeof(TemplateListView), typeof(TemplateListViewModel)),

    new("//serverlistview", typeof(ServerListView), typeof(ServerListViewModel)),
    new("//serverlistview/detail", typeof(ServerDetailView), typeof(ServerDetailViewModel)),

    new("//home/createtimesheet", typeof(TimesheetCreateView), typeof(TimesheetCreateViewModel)),
    new("//home/createtimesheet/choosecustomer", typeof(ChooseItemView), typeof(CustomerChooseTimesheetViewModel)),
    new("//home/createtimesheet/chooseproject", typeof(ChooseItemView), typeof(ProjectChooseTimesheetViewModel)),
    new("//home/createtimesheet/chooseactivity", typeof(ChooseItemView), typeof(ActivityChooseTimesheetViewModel)),
    
    new("//timesheets", typeof(TimesheetListView), typeof(TimesheetListViewModel)),
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

    public string GetRouteByRoute(string passedRoute) 
    => Routes.First(route => route.Route == passedRoute).Route;
}

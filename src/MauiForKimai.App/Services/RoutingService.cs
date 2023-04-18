
using MauiForKimai.ViewModels;
using MauiForKimai.ViewModels.Settings;
using MauiForKimai.Views;
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


    new("//favourites", typeof(FavouritesListView), typeof(FavouritesListViewModel)),
    new("//favourites/detail", typeof(FavouritesDetailView), typeof(FavouritesDetailViewModel)),
    new("//favourites/detail/choosecustomer", typeof(ChooseItemView), typeof(CustomerChooseFavouriteViewModel)),
    new("//favourites/detail/chooseproject", typeof(ChooseItemView), typeof(ProjectChooseFavouriteViewModel)),
    new("//favourites/detail/chooseactivity", typeof(ChooseItemView), typeof(ActivityChooseFavouriteViewModel)),


    new("//timesheets", typeof(TimesheetListView), typeof(TimesheetListViewModel)),
    new("//timesheets/detail", typeof(TimesheetDetailView), typeof(TimesheetDetailAllViewModel)),
    new("//timesheets/detail/choosecustomer", typeof(ChooseItemView), typeof(CustomerChooseDetailAllViewModel)),
    new("//timesheets/detail/chooseproject", typeof(ChooseItemView), typeof(ProjectChooseDetailAllViewModel)),
    new("//timesheets/detail/chooseactivity", typeof(ChooseItemView), typeof(ActivityChooseDetailAllViewModel)),

    new("//reports", typeof(ReportsView), typeof(ReportsViewModel)),
    new("///reports", typeof(ReportsView), typeof(ReportsViewModel)),


    new("//serverlistview", typeof(ServerListView), typeof(ServerListViewModel)),
    new("//serverlistview/detail", typeof(ServerDetailView), typeof(ServerDetailViewModel)),

    new("//home/timesheet", typeof(TimesheetDetailView), typeof(TimesheetDetailViewModel)),
    new("//home/timesheet/choosecustomer", typeof(ChooseItemView), typeof(CustomerChooseRecentTimesheetViewModel)),
    new("//home/timesheet/chooseproject", typeof(ChooseItemView), typeof(ProjectChooseRecentTimesheetViewModel)),
    new("//home/timesheet/chooseactivity", typeof(ChooseItemView), typeof(ActivityChooseRecentTimesheetViewModel)),
     new("//settings", typeof(SettingsView), typeof(SettingsViewModel))
    

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

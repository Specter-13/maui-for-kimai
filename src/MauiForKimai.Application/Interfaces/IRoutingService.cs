    
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Interfaces;

public interface IRoutingService
{
     IEnumerable<RouteModel> Routes { get; }

    string GetRouteByViewModel<TViewModel>() where TViewModel : IViewModel;
}

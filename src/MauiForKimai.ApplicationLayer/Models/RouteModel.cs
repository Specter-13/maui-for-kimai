using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Core.Models;

public record RouteModel(string Route, Type ViewType, Type ViewModelType)
{
}

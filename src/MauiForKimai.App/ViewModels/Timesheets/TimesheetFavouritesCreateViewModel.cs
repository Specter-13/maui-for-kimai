using MauiForKimai.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ViewModels;
public partial class TimesheetFavouritesCreateViewModel : ViewModelBase
{
    public TimesheetFavouritesCreateViewModel(IRoutingService rs, ILoginService ls) : base(rs, ls)
    {

        
    }

    [ObservableProperty]
    public TimesheetFavouriteEntity favourite;



}

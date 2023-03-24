using MauiForKimai.ApiClient.Authentication;
using MauiForKimai.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ViewModels;
public partial class TimeSheetViewModel : ViewModelBase
{
    public TimeSheetViewModel(IRoutingService rs, ILoginService ls) : base(rs, ls)
    {
    }
}

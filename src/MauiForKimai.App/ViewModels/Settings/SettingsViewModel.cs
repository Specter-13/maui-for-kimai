using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ViewModels.Settings;
public class SettingsViewModel : ViewModelBase, IViewModelSingleton
{
    public SettingsViewModel(IRoutingService rs, ILoginService ls) : base(rs, ls)
    {
    }
}

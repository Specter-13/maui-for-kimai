﻿using MauiForKimai.ApiClient.ApiClient;
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
    public TimeSheetViewModel(ApiStateProvider asp, IRoutingService routingService) : base(asp, routingService)
    {
    }
}

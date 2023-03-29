﻿using CommunityToolkit.Mvvm.Messaging;
using MauiForKimai.Messenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ViewModels;
public partial class CustomerChooseFavouriteViewModel : ChooseItemViewModel
{
    public CustomerChooseFavouriteViewModel(IRoutingService rs, ILoginService ls, ICustomerService customerService, IActivityService activityService, IProjectService projectService) : base(rs, ls, customerService, activityService, projectService)
    {
    }
}

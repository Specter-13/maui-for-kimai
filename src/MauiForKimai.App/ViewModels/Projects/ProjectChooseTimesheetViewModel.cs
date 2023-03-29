using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ViewModels;
public partial class ProjectChooseTimesheetViewModel : ChooseItemViewModel
{
    public ProjectChooseTimesheetViewModel(IRoutingService rs, ILoginService ls, ICustomerService customerService, IActivityService activityService, IProjectService projectService) : base(rs, ls, customerService, activityService, projectService)
    {
    }
}

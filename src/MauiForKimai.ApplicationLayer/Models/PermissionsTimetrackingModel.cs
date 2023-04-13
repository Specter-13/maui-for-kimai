using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Core.Models;


public class PermissionsTimetrackingModel
{

    public PermissionsTimetrackingModel(bool canSetBillable, bool canSetExport, bool canSetRate, bool hasgitlabPlugin)
    {
        CanEditBillable = canSetBillable;
        CanEditExport = canSetExport;
        CanEditRate = canSetRate;
        HasGitlabPlugin = hasgitlabPlugin;
    }
    public bool CanEditBillable { get; set; }
    public bool CanEditExport { get; set; }
    public bool CanEditRate { get; set; }

    public bool HasGitlabPlugin { get; set; }
}

using MauiForKimai.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Core.Models;
public class ServerModel
{
    public int Id { get; set; }
    public string Url { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
    public string ApiPasswordKey { get; set; }
    public bool IsDefault { get; set; }

    public bool CanEditBillable { get; set; }
    public bool CanEditExport { get; set; }
    public bool CanEditRate { get; set; }

    public bool HasGitlabPlugin { get; set; }
  
}

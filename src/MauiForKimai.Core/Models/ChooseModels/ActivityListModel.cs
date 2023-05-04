using MauiForKimai.Core.Enums;
using MauiForKimai.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MauiForKimai.Core.Models;

public class ActivityListModel : IChooseItem
{
    public ActivityListModel()
    {
        
    }
    public ActivityListModel(int id, string name, bool? billable, string color)
    {
        Id = id; 
        Name = name;
        Billable = billable;
        Color = color;
    }
    
    public int Id { get; set; }
    public string Name { get; set; }

    public bool? Billable { get; set; }

    public int ProjectId { get; set; }

    public string Color { get; set; }


}

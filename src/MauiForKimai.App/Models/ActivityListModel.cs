using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MauiForKimai.Models;

public class ActivityListModel
{
    public ActivityListModel(int id, string name)
    {
        Id = id; 
        Name = name;
    }
    
    public int Id { get; set; }
    public string Name { get; set; }
   

}

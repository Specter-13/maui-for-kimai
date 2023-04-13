using MauiForKimai.Core.Enums;
using MauiForKimai.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Core.Models;
public class CustomerListModel : IChooseItem
{
    public CustomerListModel()
    {
        
    }
    public CustomerListModel(int id, string name, bool billable)
    {
        Id = id; 
        Name = name;
        Billable = billable;
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public bool Billable { get; set; }

}

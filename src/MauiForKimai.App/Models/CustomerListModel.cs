using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Models;
public class CustomerListModel
{
    public CustomerListModel(int id, string name)
    {
        Id = id; 
        Name = name;
    }
    public int Id { get; set; }
    public string Name { get; set; }
}

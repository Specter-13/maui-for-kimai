using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Core.Models;

public class ProjectListModel
{
    public ProjectListModel(int id, string name, int customerId, bool billable)
    {
        Id = id; 
        Name = name;
        CustomerId = customerId;
        Billable = billable;
    }
    public int Id { get; set; }
    public string Name { get; set; }

    public int CustomerId { get; set; }
    public bool Billable { get; set; }
}

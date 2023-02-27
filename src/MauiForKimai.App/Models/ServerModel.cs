using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Models;
public class ServerModel
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
    public string ApiPasswordKey { get; set; }
    public bool IsDefault { get; set; }
}

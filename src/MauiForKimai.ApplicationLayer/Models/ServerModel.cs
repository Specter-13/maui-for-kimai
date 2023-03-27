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

    public static explicit operator ServerModel(ServerEntity enitty)
    {
        return new ServerModel
        {
            Id = enitty.Id,
            Url = enitty.Url,
            Name = enitty.Name,
            Username = enitty.Username,
            IsDefault = enitty.IsDefault,

        };
    }
}

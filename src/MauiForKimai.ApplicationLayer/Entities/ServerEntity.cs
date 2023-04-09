﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Core.Entities;
public class ServerEntity
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Url { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
    public bool IsDefault { get; set; }

    //public bool CanSetBillable { get; set; }
    //public bool CanSetExport { get; set; }
    //public bool CanSetRate { get; set; }

  }

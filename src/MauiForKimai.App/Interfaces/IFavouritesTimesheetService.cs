﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Interfaces;
public interface IFavouritesTimesheetService :ICrudOperationsMySql<TimesheetFavouriteEntity,TimesheetFavouriteEntity>
{
    Task ReInit();
    void DeleteDatabase(string path);
}

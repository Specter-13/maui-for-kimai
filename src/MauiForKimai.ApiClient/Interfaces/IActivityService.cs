﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ApiClient.Interfaces;
public interface IActivityService
{
     Task<ICollection<ActivityCollection>> GetActivities();
}

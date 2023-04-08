using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Wrappers;
public class StatisticsWrapper
{
    public StatisticsWrapper(string today)
    {
        Today = today;
    }

    public string Today { get; set; }
}

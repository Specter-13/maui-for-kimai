using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Wrappers;
public class BarChartDataWrapper
{
    public BarChartDataWrapper(Dictionary<string,float> data, int barWidth, float max)
    {
        Data = data;
        BarWidth = barWidth;
        Max = max;

    }
    public Dictionary<string,float> Data { get; set; }

    public int BarWidth { get; set; }

    public float Max { get; set; }
}


public class TodayWeekMonthWrapper
{
    public TodayWeekMonthWrapper(BarChartDataWrapper today, BarChartDataWrapper week, BarChartDataWrapper month)
    {
        Today = today;
        Week = week;
        Month = month;
    }
    public BarChartDataWrapper Today { get; set; }
    public BarChartDataWrapper Week { get; set; }
    public BarChartDataWrapper Month { get; set; }

}
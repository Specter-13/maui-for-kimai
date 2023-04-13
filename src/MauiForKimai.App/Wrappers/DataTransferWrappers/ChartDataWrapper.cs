using LiveChartsCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Wrappers;
public class ChartDataWrapper
{

    public ChartDataWrapper(Dictionary<string, int>  data, int width)
    {
        Data = data;
        Width = width;
    }
    public Dictionary<string, int>  Data { get; set; }
    public int Width { get; set; }
}


public class ChartSeriesWrapper
{

    public ChartSeriesWrapper(ObservableCollection<ISeries>  series, int width, IList<string> labels)
    {
        Series = series;
        ChartWidth = width;
        Labels = labels;
    }
    public ObservableCollection<ISeries>  Series { get; set; }
    public int ChartWidth { get; set; }

    public IList<string> Labels { get; set; }
}

using System.Security;
using System.Text;
using System.Xml.Linq;

namespace MauiForKiami.App.Charts;

internal class BarChartDrawable : View, IDrawable
{
    public Dictionary<string, float> Points
    {
        get => _points;
        set
        { 
            _points = value;
            OnPropertyChanged();
        }
    }

    public double XAxisScale
    {
        get => _xAxisScale;
        set { 
        _xAxisScale = value;
            OnPropertyChanged();
        }
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        
        if(Points.Count == 0) 
        {
            Max = 0.0f;
        }
        else
        { 
            Max = Points.Select(x => x.Value).Max() * 1.3f;
        }
       
        
        const int BAR_WIDTH = 35;

        canvas.FontColor = Color.FromArgb("#7F2CF6");

        _chartWidth = dirtyRect.Width;

        //If the slider was moved then change x axis for the first bar
        if (XAxisScale != XAxisScaleOrigin)
            _firstBarXAxis += (float)(XAxisScale - XAxisScaleOrigin) * _chartWidth * -1;

        var barXAxis = _firstBarXAxis;
        //passing RGB ints to constructor does not work, should I submit a PR??
        var transparentMauiPurpleColor = Color.FromRgba(178, 127, 255, 0.05);
        var mauiPurpleColor = Color.FromRgb(178, 127, 255);

        var linearGradientPaint = new LinearGradientPaint
        {
            StartColor = mauiPurpleColor,
            EndColor = transparentMauiPurpleColor,
            StartPoint = new Point(0.5, 0),
            EndPoint = new Point(0.5, 0.9)
        };

        canvas.SetFillPaint(linearGradientPaint, dirtyRect);

        for (var i = 0; i < Points.Count; i++)
        {
            var point = Points.ElementAt(i);
            if(point.Key == "") continue;
            var barHeight = dirtyRect.Height - (dirtyRect.Height * (point.Value / Max) * BarScale);

            //Draw bars
            canvas.FillRectangle(barXAxis, barHeight, BAR_WIDTH, dirtyRect.Height - barHeight);
            canvas.FontSize = 11;
            //Draw text
            var name = SplitLongStrings(point.Key);

            var denormalizeValue = point.Value * 1000;
            var duration = TimeSpan.FromSeconds(denormalizeValue);
            var durationString = $"{((int)duration.TotalHours).ToString("00")}:{duration.Minutes.ToString("00")}";
            canvas.FontSize = 15;
     
            
            canvas.DrawString(durationString, barXAxis + 12,  dirtyRect.Height - 1, HorizontalAlignment.Center);

            canvas.FontSize = 12;
            canvas.DrawString(name, barXAxis + 12, barHeight - 30, HorizontalAlignment.Center);
            //Draw text
           
          
            barXAxis += BAR_WIDTH + 55;
        }

        XAxisScaleOrigin = XAxisScale;
    }

    public float Max;
    public float BarScale = 0.0f;
    public double XAxisScaleOrigin;
    public bool ChartsLoading = true;

    private float _chartWidth;
    private double _xAxisScale;
    private float _firstBarXAxis = 40.0f;
    private Dictionary<string, float> _points = new Dictionary<string, float> {{"",0}};


    private string SplitLongStrings(string name)
    { 
        string[] words = name.Split(' ');
        StringBuilder sb = new StringBuilder();
        int currLength = 0;
        foreach(string word in words)
        {
            if(currLength + word.Length + 1 < 15) // +1 accounts for adding a space
            {
              sb.AppendFormat(" {0}", word);
              currLength = (sb.Length % 20);
            }
            else
            {
              sb.AppendFormat("{0} {1}", Environment.NewLine, word);
              currLength = 0;
            }
        }

        return sb.ToString();
    }
}
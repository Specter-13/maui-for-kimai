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
        const int BAR_WIDTH = 30;

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
            EndPoint = new Point(0.5, 1)
        };

        canvas.SetFillPaint(linearGradientPaint, dirtyRect);

        for (var i = 0; i < Points.Count; i++)
        {
            var point = Points.ElementAt(i);
            var barHeight = dirtyRect.Height - (dirtyRect.Height * (point.Value / Max) * BarScale);

            //Draw bars
            canvas.FillRectangle(barXAxis, barHeight, BAR_WIDTH, dirtyRect.Height - barHeight);
            canvas.FontSize = 11;
            //Draw text
            var name = SplitLongStrings(point.Key);

            canvas.DrawString(name, barXAxis + 11, barHeight - 20, HorizontalAlignment.Center);
            //Draw text
           
            var duration = TimeSpan.FromSeconds(point.Value);
            var durationString = $"{((int)duration.TotalHours).ToString("00")}:{duration.Minutes.ToString("00")}";
            canvas.FontSize = 15;
            canvas.DrawString(durationString, barXAxis -2, dirtyRect.Height + 13, HorizontalAlignment.Left);
      
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
    private Dictionary<string, float> _points;


    private string SplitLongStrings(string name)
    { 
        string[] words = name.Split(' ');
        StringBuilder sb = new StringBuilder();
        int currLength = 0;
        foreach(string word in words)
        {
            if(currLength + word.Length + 1 < 20) // +1 accounts for adding a space
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
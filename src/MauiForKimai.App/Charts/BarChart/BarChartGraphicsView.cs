using Microsoft.Maui.Controls;

namespace MauiForKiami.App.Charts;

internal class BarChartGraphicsView : GraphicsView
{
    public static readonly BindableProperty XAxisScaleProperty = BindableProperty.Create(nameof(XAxisScale),
    typeof(double),
    typeof(BarChartGraphicsView),
    0.0,
    propertyChanged: (b, o, n) => {
        var graphicsView = ((BarChartGraphicsView)b);
        graphicsView.BarChartDrawable.XAxisScale = Convert.ToSingle(n);
        graphicsView.Invalidate();
    });

    public double XAxisScale
    {
        get => (double)GetValue(XAxisScaleProperty);
        set => SetValue(XAxisScaleProperty, value);
    }

    public static readonly BindableProperty PointsProperty = BindableProperty.Create(nameof(Points),
        typeof(Dictionary<string, float>),
        typeof(BarChartGraphicsView),
        new Dictionary<string, float>(),
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            var points = (Dictionary<string, float>)newValue;
            if(points == null || points.Count == 0)
            { 
                return;    
            }


            var chartView = (BarChartGraphicsView)bindable;

            //You can add/remove new data points which will redraw bar chart
            chartView.BarChartDrawable.Points = (Dictionary<string, float>)newValue;
            chartView.Invalidate();
        });

    public Dictionary<string, float> Points
    {
        get => (Dictionary<string, float>)GetValue(PointsProperty);
        set => SetValue(PointsProperty, value);
    }

    public BarChartGraphicsView()
    {
       
        
        Drawable = BarChartDrawable;

        //This is not working...
        LoadChartAnimation();
    }

    /// <summary>
    /// Animates bars from 1/30 scale over 1 second
    /// </summary>
    public void LoadChartAnimation()
    {
        for (var i = 0; i <= 30; i++)
        {
            BarChartDrawable.BarScale = i / 30f;
            Invalidate();
            Task.Delay(33);
        }
        BarChartDrawable.ChartsLoading = false;
    }

    public BarChartDrawable BarChartDrawable = new BarChartDrawable();
}

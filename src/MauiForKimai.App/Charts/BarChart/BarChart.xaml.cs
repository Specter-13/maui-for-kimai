using CommunityToolkit.Mvvm.Messaging;
using MauiForKimai.Messenger;
using MauiForKimai.Wrappers;

namespace MauiForKiami.App.Charts;

public partial class BarChart : StackLayout
{
    public static readonly BindableProperty PointsProperty = BindableProperty.Create(nameof(Points),
            typeof(Dictionary<string, float>),
            typeof(BarChart),
            new Dictionary<string, float>(),
            propertyChanged: async (bindable, oldValue, newValue) =>
            {
                var chartView = ((BarChart)bindable);

                chartView.Chart.BarChartDrawable.Points = (Dictionary<string, float>)newValue;
                //chartView.ChartDesktop.BarChartDrawable.Points = (Dictionary<string, float>)newValue;
            });






     public static readonly BindableProperty CustomWidthProperty = BindableProperty.Create(nameof(CustomWidth),
            typeof(double),
            typeof(BarChart),    
            propertyChanged: async (bindable, oldValue, newValue) =>
            {
                var chartView = ((BarChart)bindable);

                chartView.Chart.WidthRequest = (double)newValue;
              
            });

    
     public static readonly BindableProperty CustomHeightProperty = BindableProperty.Create(nameof(CustomHeight),
            typeof(double),
            typeof(BarChart),   
            propertyChanged: async (bindable, oldValue, newValue) =>
            {
                var chartView = ((BarChart)bindable);

                chartView.Chart.HeightRequest = (double)newValue;
              
            });
    public Dictionary<string, float> Points
    {
        get => (Dictionary<string, float>)GetValue(PointsProperty);
        set => SetValue(PointsProperty, value);
    }

    public double CustomWidth
    {
        get => (double)GetValue(CustomWidthProperty);
        set => SetValue(CustomWidthProperty, value);
    }

    public double CustomHeight
    {
        get => (double)GetValue(CustomHeightProperty);
        set => SetValue(CustomHeightProperty, value);
    }


    public BarChart()
    { 
        
        InitializeComponent();

        BindingContext = this;
        RegisterMessages();
    }
    private void RegisterMessages()
	{ 
		 WeakReferenceMessenger.Default.Register<TodayWeekMonthWrapper>(this, (r, m) =>
        {
			this.Chart.Invalidate();
        });
	}
   
}
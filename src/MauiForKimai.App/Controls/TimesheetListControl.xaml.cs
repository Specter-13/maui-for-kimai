namespace MauiForKimai.Controls;

public partial class TimesheetListControl : ContentView
{
	public static readonly BindableProperty TimesheetProperty = BindableProperty.Create(nameof(TimesheetProperty), typeof(string), typeof(TimesheetListControl), string.Empty);

	public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(TitleProperty), typeof(string), typeof(TimesheetListControl), propertyChanged: (bindable, oldValue, newValue) =>
	{ 
		var control = (TimesheetListControl)bindable;
		var day = newValue as string;
		if(_today == day)
		{
			control.Day.Text = "Today";
		}
		else
		{ 
			control.Day.Text =day;
		}
	});


	public TimesheetListControl()
	{
		InitializeComponent();
	}

	public TimesheetModel Timesheet
	{
		get => GetValue(TimesheetProperty) as TimesheetModel;
		set => SetValue(TimesheetProperty, value);
	}

	public string Title
	{
		get => GetValue(TitleProperty) as string;
		set => SetValue(TitleProperty, value);
	}

    public static string _today => DateTime.Today.ToShortDateString();

}
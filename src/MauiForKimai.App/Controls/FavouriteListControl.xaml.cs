namespace MauiForKimai.Controls;

public partial class FavouriteListControl : ContentView
{
	public static readonly BindableProperty TimesheetProperty = BindableProperty.Create(nameof(TimesheetProperty), typeof(string), typeof(TimesheetListControl), string.Empty);
	public FavouriteListControl()
	{
		InitializeComponent();
	}
	public TimesheetModel Timesheet
	{
		get => GetValue(TimesheetProperty) as TimesheetModel;
		set => SetValue(TimesheetProperty, value);
	}

}
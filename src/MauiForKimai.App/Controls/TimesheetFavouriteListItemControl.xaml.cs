namespace MauiForKimai.Controls;

public partial class TimesheetFavouriteListItemControl : ContentView
{
	public static readonly BindableProperty TimesheetProperty = BindableProperty.Create(nameof(TimesheetProperty), typeof(string), typeof(TimesheetListItemControl), string.Empty);
	public TimesheetFavouriteListItemControl()
	{
		InitializeComponent();
	}
	public TimesheetListItemModel Timesheet
	{
		get => GetValue(TimesheetProperty) as TimesheetListItemModel;
		set => SetValue(TimesheetProperty, value);
	}

}
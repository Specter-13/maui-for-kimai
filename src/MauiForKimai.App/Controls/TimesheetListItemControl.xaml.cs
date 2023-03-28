namespace MauiForKimai.Controls;

public partial class TimesheetListItemControl : ContentView
{
	public static readonly BindableProperty TimehseetProperty = BindableProperty.Create(nameof(TimehseetProperty), typeof(string), typeof(TimesheetListItemControl), string.Empty);
   
	public TimesheetListItemControl()
	{
		InitializeComponent();
	}

	public TimesheetRecentListModel Timesheet
	{
		get => GetValue(TimehseetProperty) as TimesheetRecentListModel;
		set => SetValue(TimehseetProperty, value);
	}
}
namespace MauiForKimai.Controls;

public partial class TimesheetListItemControl : ContentView
{
	public static readonly BindableProperty TimesheetListItemControlProperty  = BindableProperty.Create(nameof(Timesheet),typeof(TimesheetCollectionExpanded), typeof(TimesheetListItemControl), propertyChanged:(bindable,oldvalue,newvalue) =>
	{ 
		});
	public TimesheetListItemControl()
	{
		InitializeComponent();
	}

	public TimesheetCollectionExpanded Timesheet
	{
		get => GetValue(TimesheetListItemControlProperty) as TimesheetCollectionExpanded;
		set => SetValue(TimesheetListItemControlProperty, value);
	}
}
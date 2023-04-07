namespace MauiForKimai.Views;

public partial class ActivityReportsView 
{
	public ActivityReportsView(ReportsViewModel vm)
	{
		InitializeComponent();
		BindingContext =vm;
	}
}
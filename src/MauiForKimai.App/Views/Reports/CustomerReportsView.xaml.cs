namespace MauiForKimai.Views;

public partial class CustomerReportsView 
{
	public CustomerReportsView(ReportsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}
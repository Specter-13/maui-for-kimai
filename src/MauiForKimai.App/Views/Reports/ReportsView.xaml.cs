using MauiForKimai.ViewModels.Reports;

namespace MauiForKimai.Views;

public partial class ReportsView
{
	public ReportsView(ReportsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}
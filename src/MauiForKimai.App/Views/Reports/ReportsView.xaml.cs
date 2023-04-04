using CommunityToolkit.Mvvm.Messaging;
using MauiForKiami.App.Charts;
using MauiForKimai.Messenger;
using MauiForKimai.ViewModels;

namespace MauiForKimai.Views;

public partial class ReportsView
{
	private ReportsViewModel _vm;
	public ReportsView(ReportsViewModel vm)
	{
		InitializeComponent();
		_vm = vm;
		BindingContext = _vm;
		RegisterMessages();
		
	}

	public void AddChart(int width)
	{ 
		var barChart = new BarChart();
		barChart.BindingContext = _vm.Data;
		barChart.Points = _vm.Data;
		barChart.CustomWidth = width;
		barChart.CustomHeight = 300;
		barChart.HorizontalOptions=LayoutOptions.Fill;
		this.MyVerticalStack.Add(barChart);
	}


	private void RegisterMessages()
	{ 
		 WeakReferenceMessenger.Default.Register<ChartLoadMessage>(this, (r, m) =>
        {
			var width = m.Value;
			AddChart(width);
			//await TryToGetActiveTimesheet();
        });
	}

}
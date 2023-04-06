using CommunityToolkit.Mvvm.Messaging;
using MauiForKiami.App.Charts;
using MauiForKimai.Messenger;
using MauiForKimai.ViewModels;
using MauiForKimai.Wrappers;
using Microsoft.Maui.Platform;

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

	public void ReloadData(TodayWeekMonthWrapper wrapper)
	{
		//set data again when are available, because there is a problem to draw graph with empty values

		this.BarChartToday.Points = wrapper.Today.Data;
		this.BarChartToday.CustomWidth = wrapper.Today.BarWidth;
		this.BarChartToday.MaxValue = wrapper.Today.Max;

		this.BarChartWeek.Points = wrapper.Week.Data;
		this.BarChartWeek.CustomWidth = wrapper.Week.BarWidth;
		this.BarChartWeek.MaxValue = wrapper.Week.Max;
		
		this.BarChartMonth.Points = wrapper.Month.Data;
		this.BarChartMonth.CustomWidth = wrapper.Month.BarWidth;
		this.BarChartMonth.MaxValue = wrapper.Month.Max;
	}


	private void RegisterMessages()
	{ 
		 WeakReferenceMessenger.Default.Register<TodayWeekMonthWrapper>(this, (r, m) =>
        {
			//(BarChart)
			ReloadData(m);
        });
	}

}
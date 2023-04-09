using CommunityToolkit.Mvvm.Messaging;
using MauiForKimai.Messenger;
using MauiForKimai.ViewModels;
using MauiForKimai.Wrappers;
using Microsoft.Maui.Platform;

namespace MauiForKimai.Views;

public partial class ReportsView
{
	public ReportsView(ReportsViewModel vm)
	{

		InitializeComponent();
		BindingContext = vm;
		
	}

}
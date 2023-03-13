using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiForKimai.ApiClient.ApiClient;
using MauiForKimai.ApiClient.Authentication;
using MauiForKimai.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ViewModels.Base;
public abstract partial class ViewModelBase : ObservableObject, IViewModel
{
	
	public ViewModelBase(ApiStateProvider asp, IRoutingService routingService)
	{
		apiStateProvider = asp;
		RoutingService = routingService;
	}

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(IsNotBusy))]
	bool isBusy;


	[ObservableProperty]
	public ApiStateProvider apiStateProvider;

	public IRoutingService RoutingService { get; }

	public bool IsNotBusy => !IsBusy;


	



}

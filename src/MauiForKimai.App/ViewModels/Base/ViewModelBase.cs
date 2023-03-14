using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiForKimai.ApiClient.Authentication;
using MauiForKimai.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyMvvm;

namespace MauiForKimai.ViewModels.Base;
public abstract partial class ViewModelBase : TinyViewModel, IViewModel
{
	
	public ViewModelBase(ApiStateProvider asp, IRoutingService routingService)
	{
		apiStateProvider = asp;
		RoutingService = routingService;
	}


	[ObservableProperty]
	public ApiStateProvider apiStateProvider;

	public IRoutingService RoutingService { get; }



	



}

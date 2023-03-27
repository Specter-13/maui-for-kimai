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
	
	protected ILoginService loginService {get; }
	protected IRoutingService routingService { get; }
	public ViewModelBase(IRoutingService rs, ILoginService ls)
	{
		routingService = rs;
		loginService = ls;
		ApiStateProvider = loginService.GetApiStateProvider();
	}

    [ObservableProperty]
	ApiStateProvider apiStateProvider;

	//virtual for testing purposes
    public virtual NetworkAccess GetConnectivity()
		=> Connectivity.Current.NetworkAccess;




}

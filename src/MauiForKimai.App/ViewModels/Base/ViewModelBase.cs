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
public partial class ViewModelBase : ObservableObject
{
	
	public ViewModelBase(ApiStateProvider asp)
	{
		apiStateProvider = asp;
	}

	[ObservableProperty]
	public ApiStateProvider apiStateProvider;


	



}

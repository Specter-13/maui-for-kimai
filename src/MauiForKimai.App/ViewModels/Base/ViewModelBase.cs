using CommunityToolkit.Mvvm.ComponentModel;
using MauiForKimai.ApiClient.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ViewModels.Base;
public partial class ViewModelBase : ObservableObject
{
	
	public ViewModelBase(AuthHandler auth)
	{
		authHandler = auth;
	}

	[ObservableProperty]
	AuthHandler authHandler;
}

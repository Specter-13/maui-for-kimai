using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiForKimai.ApiClient.ApiClient;
using MauiForKimai.ApiClient.Authentication;
using MauiForKimai.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ViewModels;
public partial class ServerDetailViewModel : ViewModelBase
{
	public ServerDetailViewModel(ApiStateProvider asp) : base(asp)
	{

	}
    [ObservableProperty]
	private string name;
    [ObservableProperty]
	private string url;
    [ObservableProperty]
	private string username;
	[ObservableProperty]
	private string apiPassword;
	[ObservableProperty]
	private string isDefault;

	//[RelayCommand]
 //   async Task Add()
 //   { 
		
	//}
}

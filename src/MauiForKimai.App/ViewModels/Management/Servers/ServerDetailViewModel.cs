using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

    public ServerDetailViewModel(IRoutingService rs, ILoginService ls) : base(rs, ls)
    {
    }

    //[RelayCommand]
    //   async Task Add()
    //   { 

    //}
}

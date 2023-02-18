using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiForKimai.BL.ApiClient;
using MauiForKimai.BL.Facades;
using MauiForKimai.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MauiForKimai.ViewModels;

public partial class MainViewModel : ViewModelBase
{
	public IUserClient UserClient { get; set; }
	private UserFacade _fc { get; set; }
	public List<UserCollection> Users { get; set; }
	public MainViewModel(UserFacade fc)
	{
		_fc = fc;
	}
	



	[ObservableProperty]
	private string kimaiUrl;
	[ObservableProperty]
	private string userName;
	[ObservableProperty]
	private string password;

	
	[ObservableProperty]
	private string firstUser;

	[ObservableProperty]
	private string version;

   

    [RelayCommand]
    async Task Login()
    {
		
		var httpClient = new HttpClient();
		KimaiUrl = "https://specter13maui.kimai.cloud/";
		UserName = "dadkos34@gmail.com";
		Password= "internet";

		httpClient.BaseAddress = new Uri(KimaiUrl);
		httpClient.DefaultRequestHeaders.Add("X-AUTH-USER", UserName);
		httpClient.DefaultRequestHeaders.Add("X-AUTH-TOKEN", Password);

		var api = ApiClientSingleton.CreateInstance(httpClient);
		
		Version = (await api.DefaultClient.VersionAsync()).Version1;

		FirstUser = (await _fc.GetAllUsersAsync()).First().Username;

        await Task.Delay(1000);   

		string text = "Connection to Kimai established!";
		ToastDuration duration = ToastDuration.Short;
		double fontSize = 14;

		var toast = Toast.Make(text, duration, fontSize);
		CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
		await toast.Show(cancellationTokenSource.Token);
        
    }
}


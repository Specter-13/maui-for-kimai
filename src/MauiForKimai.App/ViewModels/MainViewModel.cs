using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiForKimai.ApiClient.Authentication;
using MauiForKimai.ViewModels.Base;
using MauiForKimai.ApiClient.Interfaces;

namespace MauiForKimai.ViewModels;

public partial class MainViewModel : ViewModelBase
{
	private readonly IUserService _userService;
	private readonly AuthHandler _autHandler;
	public MainViewModel( IUserService userService, AuthHandler aut)
	{
		_userService = userService;
		_autHandler = aut;
	}
	
	[ObservableProperty]
	private string description;


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
		
	
		
		KimaiUrl = "https://specter13maui.kimai.cloud/";
		UserName = "dadkos34@gmail.com";
		Password= "internet";

		//var authHandler = new AuthHandler();
		//authHandler.SetAccessTokens(UserName, Password);

		
		_autHandler.SetBaseUrl(KimaiUrl);
		_autHandler.SetAccessTokens(UserName,Password);
		


		//httpClient.BaseAddress = new Uri(KimaiUrl);

		//KimaiApiManager.CreateInstance(httpClient);


		FirstUser = (await _userService.GetAllUsersAsync()).First().Username;
		
		//Version = (await KimaiApiManager.ApiClient.DefaultClient.VersionAsync()).Version1;

		//FirstUser = (await  KimaiApiManager.ApiClient.UserClient.UsersAllAsync()).First().Username;

        await Task.Delay(1000);   

		string text = "Connection to Kimai established!";
		ToastDuration duration = ToastDuration.Short;
		double fontSize = 14;

		var toast = Toast.Make(text, duration, fontSize);
		CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
		await toast.Show(cancellationTokenSource.Token);
        
    }
}


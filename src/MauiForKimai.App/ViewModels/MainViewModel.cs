using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiForKimai.ApiClient.Authentication;
using MauiForKimai.ViewModels.Base;
using MauiForKimai.ApiClient.Interfaces;
using CommunityToolkit.Mvvm.Messaging;
using MauiForKimai.ApplicationLayer.Messages;

namespace MauiForKimai.ViewModels;

public partial class MainViewModel : ViewModelBase
{
	private readonly IUserService _userService;
	private readonly AuthHandler _autHandler;
	public MainViewModel(IUserService userService, AuthHandler aut) : base(aut)
	{
		_userService = userService;
		_autHandler = aut;

		//WeakReferenceMessenger.Default.Register<LoginAttemptMessage>(this, (r, m) =>
		//{
		//	var x = 10;
			
		//	// Handle the message here, with r being the recipient and m being the
		//	// input message. Using the recipient passed as input makes it so that
		//	// the lambda expression doesn't capture "this", improving performance.
		//});
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

		
		_autHandler.SetAuthInfo(KimaiUrl,UserName,Password);
		_autHandler.SetIsAuthenticated();
		


		//httpClient.BaseAddress = new Uri(KimaiUrl);

		//KimaiApiManager.CreateInstance(httpClient);


		FirstUser = (await _userService.GetAllUsersAsync()).First().Username;
		
		//Version = (await KimaiApiManager.ApiClient.DefaultClient.VersionAsync()).Version1;

		//FirstUser = (await  KimaiApiManager.ApiClient.UserClient.UsersAllAsync()).First().Username;


		string text = "Connection to Kimai established!";
		ToastDuration duration = ToastDuration.Short;
		double fontSize = 14;

		var toast = Toast.Make(text, duration, fontSize);
		CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
		await toast.Show(cancellationTokenSource.Token);
        
    }
}


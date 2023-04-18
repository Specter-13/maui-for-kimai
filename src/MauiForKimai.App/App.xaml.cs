

using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using MauiForKimai.ApiClient.Authentication;
using MauiForKimai.Helpers;
using MauiForKimai.Services;
using MauiForKimai.Shells;
using MauiForKimai.ViewModels;
using MauiForKimai.ViewModels.Base;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using TinyMvvm;

namespace MauiForKimai;

public partial class App : TinyApplication
{
	private readonly ILoginService _loginService;
	public App(MenuViewModel menuViewModel, ILoginService loginService)
	{

		InitializeComponent();
		_loginService = loginService;


		

		#if ANDROID || IOS
            MainPage = new AppShellMobile(menuViewModel);
		#else
            MainPage = new AppShellDesktop(menuViewModel);
		#endif
        

		// let's set the initial theme already during the app start
		SetThemeOnStartup();
		// subscribe to changes in the settings
		SettingsService.Instance.PropertyChanged += OnSettingsPropertyChanged;


	}

	private void OnSettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == nameof(SettingsService.Theme))
		{
			if(SettingsService.Instance != null) 
			{
				Preferences.Default.Set("mfk_default_theme", (int)SettingsService.Instance.Theme.AppTheme);
			}


			SetTheme();
		}
	
	}

	private void SetThemeOnStartup()
	{ 
		int value = Preferences.Default.Get("mfk_default_theme",-1);
		if (value != -1) 
		{
			if(value == 0)
				SettingsService.Instance.Theme = Theme.System;
			else if (value == 1)
				SettingsService.Instance.Theme = Theme.Light;
			else
				SettingsService.Instance.Theme = Theme.Dark;
			
		}
	}
	private void SetTheme()
	{
		try
		{
			UserAppTheme = SettingsService.Instance?.Theme != null
				? SettingsService.Instance.Theme.AppTheme
				: AppTheme.Unspecified;
		}
		catch (Exception)
		{

		}
	}

	
}

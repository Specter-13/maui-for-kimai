﻿

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
	public App()
	{

		InitializeComponent();


		#if ANDROID || IOS
            MainPage = new AppShellMobile();
		#else
            MainPage = new AppShellDesktop();
		#endif

		SetThemeOnStartup();
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

			SetTheme();
			
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

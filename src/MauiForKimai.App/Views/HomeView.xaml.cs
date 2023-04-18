using MauiForKimai.ViewModels;

namespace MauiForKimai.Views;

public partial class HomeView
{
    private readonly HomeViewModel _vm;
	public HomeView(HomeViewModel vm)
	{
		InitializeComponent();
        _vm = vm;
		BindingContext = _vm;
	}

  //  protected override async void OnAppearing()
  //  {
		//await _vm.TryToLoginToDefaultServer();
  //      base.OnAppearing();	
  //  }

   
        
    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        var myButton = (ImageButton)sender;
        if(myButton.ClassId == "StartButton")
        { 
            await start.FadeTo(0,500,Easing.Linear);
            await start.FadeTo(1,500,Easing.Linear);
        }

        if(myButton.ClassId == "StopButton")
        { 
            await stop.FadeTo(0,500,Easing.Linear);
            await stop.FadeTo(1,500,Easing.Linear);
        }
    }
}
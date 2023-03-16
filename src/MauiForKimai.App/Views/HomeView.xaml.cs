using MauiForKimai.ViewModels;

namespace MauiForKimai.Views;

public partial class HomeView
{
	private readonly HomeViewModel _vm;
	public HomeView(HomeViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    protected override void OnAppearing()
    {
		//_vm.
        base.OnAppearing();	
    }

    private void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
    {
        //if(e.StatusType == GestureStatus.Running)
        //{ 
            SwipeMenu.TranslationY = e.TotalY;
        //}
        //SwipeMenu.ScaleYTo(SwipeMenu.Scale ,250,Easing.Linear);

    }
        
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
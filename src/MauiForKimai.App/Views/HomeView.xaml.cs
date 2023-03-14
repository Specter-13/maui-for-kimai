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

}
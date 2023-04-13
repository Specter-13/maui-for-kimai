using MauiForKimai.ViewModels;

namespace MauiForKimai.Views;

public partial class FavouritesDetailView 
{
	public FavouritesDetailView(FavouritesDetailViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}
using MauiForKimai.ViewModels;

namespace MauiForKimai.Views;

public partial class FavouritesListView
{
	public FavouritesListView(FavouritesListViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}
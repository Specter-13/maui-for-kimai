

using MauiForKimai.ViewModels;

namespace MauiForKimai.Views;

public partial class CustomerChooseView
{
	private readonly CustomerChooseViewModel _vm;
	public CustomerChooseView(CustomerChooseViewModel vm)
	{
		InitializeComponent();
		_vm = vm;
		BindingContext = _vm;
	}
	private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
		_vm.FilterCommand.Execute(e.NewTextValue);
    }
}
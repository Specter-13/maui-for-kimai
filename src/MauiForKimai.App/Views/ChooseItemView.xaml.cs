namespace MauiForKimai.Views;

public partial class ChooseItemView 
{
	private readonly ChooseItemViewModel _vm;
	public ChooseItemView(ChooseItemViewModel vm)
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
using MauiForKimai.ViewModels.Activity;

namespace MauiForKimai.Views.Activity;

public partial class ActivityChooseView
{
	private readonly ActivityChooseViewModel _vm;
	public ActivityChooseView(ActivityChooseViewModel vm)
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
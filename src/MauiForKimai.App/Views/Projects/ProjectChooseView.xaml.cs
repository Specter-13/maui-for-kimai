using MauiForKimai.ViewModels.Projects;

namespace MauiForKimai.Views.Projects;

public partial class ProjectChooseView
{
	private readonly ProjectChooseViewModel _vm;
	public ProjectChooseView(ProjectChooseViewModel vm)
	{
		InitializeComponent();
		_vm = vm;
		BindingContext = _vm;
	}

	private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
		_vm.FilterProjectsCommand.Execute(e.NewTextValue);
    }
}
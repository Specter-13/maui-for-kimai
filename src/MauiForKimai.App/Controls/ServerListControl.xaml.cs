namespace MauiForKimai.Controls;

public partial class ServerListControl : ContentView
{
	public static readonly BindableProperty ServerProperty = BindableProperty.Create(nameof(ServerProperty), typeof(string), typeof(ServerListControl), string.Empty);
	public ServerListControl()
	{
		InitializeComponent();
		BindingContext = Server;
	}

	public ServerEntity Server
	{
		get => GetValue(ServerProperty) as ServerEntity;
		set => SetValue(ServerProperty, value);
	}
}
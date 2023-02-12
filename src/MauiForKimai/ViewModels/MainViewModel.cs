using MauiForKimai.BL.ApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ViewModels;

public class MainViewModel
{
	public IUserClient UserClient { get; set; }
	public List<UserCollection> Users { get; set; }
	public MainViewModel(IUserClient userClient)
	{
		UserClient = userClient;
		Task.Run(() => this.GetUsers()).Wait();
		
		FirstUserName = Users.FirstOrDefault().Username;
	}

	private async Task<List<UserCollection>> GetUsers()
	{ 
		var x = await UserClient.UsersAllAsync(null,null,null,null);
		Users = x.ToList();
		return x.ToList();
	}

	public string FirstUserName { get; set; }
	public bool MyBool { get; set; } = true;
}


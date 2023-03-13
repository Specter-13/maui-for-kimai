using CommunityToolkit.Mvvm.Messaging;
using MauiForKimai.ApiClient;
using MauiForKimai.ApiClient.Authentication;
using MauiForKimai.ApiClient.Interfaces;
using MauiForKimai.ApplicationLayer.Messages;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ApiClient.Services;
public class UserService : BaseService, IUserService
{
	//private UserClient _userClient;

	public UserService(IHttpClientFactory httpClientFactory, ApiStateProvider asp) : base(httpClientFactory,asp)
	{

	}
	//public void InitializeClient(string baseUrl)
	//{
	//	base.CreateNewHttpClient(baseUrl);
	//	//_userClient = new UserClient(base._httpClient);
	//}

	public Task<UserEntity> GetMe()
	{ 
		return ApiClient.MeAsync();
		//return _userClient.MeAsync();
		//return _userClient.Get_me_userAsync();
	}


	public Task<ICollection<UserCollection>> GetAllUsersAsync()
	{ 
		
		//return _userClient.UsersAllAsync();
		//return _userClient.Get_get_usersAsync();
		return ApiClient.UsersAllAsync(null,null,null,null);
	}

	public Task<UserEntity> GetUserByIdAsync(int id)
	{ 
		return ApiClient.UsersGETAsync(id);
		//return _userClient.UsersGETAsync(id);
		//return _userClient.Get_get_userAsync(id.ToString());
	}

	

	
}

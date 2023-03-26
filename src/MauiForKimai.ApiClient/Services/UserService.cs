using CommunityToolkit.Mvvm.Messaging;
using MauiForKimai.ApiClient;
using MauiForKimai.ApiClient.Authentication;
using MauiForKimai.ApiClient.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ApiClient.Services;
public class UserService : BaseService, IUserService
{

	public UserService(IHttpClientFactory httpClientFactory, ApiStateProvider asp) : base(httpClientFactory,asp)
	{

	}


	public Task<UserEntity> GetMe()
	{ 
		return ApiClient.MeAsync();
	}


	public Task<ICollection<UserCollection>> GetAllUsersAsync()
	{ 
		return ApiClient.UsersAllAsync(null,null,null,null);
	}

	public Task<UserEntity> GetUserByIdAsync(int id)
	{ 
		return ApiClient.UsersGETAsync(id);
	}

	

	
}

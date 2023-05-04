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

	public UserService(ApiClientWrapper aw) : base(aw)
	{

	}


	public Task<UserEntity> GetMe()
	{ 
		return _aw.ApiClient?.MeAsync();
	}


	public Task<ICollection<UserCollection>> GetAllUsersAsync()
	{ 
		return _aw.ApiClient?.UsersAllAsync(null,null,null,null);
	}

	public Task<UserEntity> GetUserByIdAsync(int id)
	{ 
		return _aw.ApiClient?.UsersGETAsync(id);
	}

	

	
}

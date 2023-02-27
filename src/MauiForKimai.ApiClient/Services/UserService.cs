using MauiForKimai.ApiClient.Authentication;
using MauiForKimai.ApiClient.Client;
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

	private IUserClient _userClient;

	public UserService(IHttpClientFactory httpClientFactory, AuthHandler auth) : base(httpClientFactory,auth)
	{
		_userClient = new UserClient(_httpClient);
	}

	public Task<ICollection<UserCollection>> GetAllUsersAsync()
	{ 
		InitializeClientIfBaseUrlIsNull();
		return _userClient.UsersAllAsync();
	}

	private void InitializeClientIfBaseUrlIsNull()
	{
		if(_httpClient.BaseAddress == null ) 
		{
			_httpClient.BaseAddress = new Uri(_auth.GetBaseUrl());
			_userClient = new UserClient(_httpClient);
		}
	}
}

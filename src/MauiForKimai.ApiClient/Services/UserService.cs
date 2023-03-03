using CommunityToolkit.Mvvm.Messaging;
using MauiForKimai.ApiClient.Authentication;
using MauiForKimai.ApiClient.Client;
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
	private IUserClient _userClient;

	public UserService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
	{

	}
	public void InitializeClient(string baseUrl)
	{
		_httpClient.BaseAddress = new Uri(baseUrl);
		 _userClient = new UserClient(_httpClient);
	}
	public Task<ICollection<UserCollection>> GetAllUsersAsync()
	{ 
		
		return _userClient.UsersAllAsync();
	}

	

	
}

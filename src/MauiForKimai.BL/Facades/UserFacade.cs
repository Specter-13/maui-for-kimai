using MauiForKimai.BL.ApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.BL.Facades;
public class UserFacade
{
	
	public UserFacade()
	{
		
	}

	public async Task<IEnumerable<UserCollection>> GetAllUsersAsync()
	{ 
		return await ApiClientSingleton.ApiClient.UserClient.UsersAllAsync(null,null,null,null);
	}
}

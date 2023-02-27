using MauiForKimai.ApiClient.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ApiClient.Interfaces;
public interface IUserService
{
    Task<ICollection<UserCollection>> GetAllUsersAsync();
}

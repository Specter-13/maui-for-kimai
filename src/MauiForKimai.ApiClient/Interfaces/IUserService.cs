using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ApiClient.Interfaces;
public interface IUserService : IBaseService
{
    Task<ICollection<UserCollection>> GetAllUsersAsync();

    Task<UserEntity> GetMe();
    Task<UserEntity> GetUserByIdAsync(int id);
}

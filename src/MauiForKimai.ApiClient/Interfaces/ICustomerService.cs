using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ApiClient.Interfaces;
public interface ICustomerService : IBaseService
{
    Task<ICollection<CustomerCollection>> GetCustomers();

    Task<CustomerEntity> GetById(int id);
}

using MauiForKimai.ApiClient.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ApiClient.Services;
public class CustomerService : BaseService, ICustomerService
{
    public CustomerService(ApiClientWrapper aw) : base(aw)
    {
    }

    public Task<ICollection<CustomerCollection>> GetCustomers()
    {
        return _aw.ApiClient?.CustomersAllAsync(null,null,null,null);
    }

    public Task<CustomerEntity> GetById(int id)
    { 
        return _aw.ApiClient?.CustomersGETAsync(id.ToString());
    }
}

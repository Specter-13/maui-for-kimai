using MauiForKimai.ApiClient.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ApiClient.Services;
public class CustomerService : BaseService, ICustomerService
{
    public CustomerService(IHttpClientFactory httpClientFactory, ApiLoginContext asp) : base(httpClientFactory, asp)
    {
    }

    public Task<ICollection<CustomerCollection>> GetCustomers()
    {
        return ApiClient?.CustomersAllAsync(null,null,null,null);
    }

    public Task<CustomerEntity> GetById(int id)
    { 
        return ApiClient?.CustomersGETAsync(id.ToString());
    }
}

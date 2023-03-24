using MauiForKimai.ApiClient.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ApiClient.Services;
public class CustomerService : BaseService, ICustomerService
{
    public CustomerService(IHttpClientFactory httpClientFactory, ApiStateProvider asp) : base(httpClientFactory, asp)
    {
    }

    public Task<ICollection<CustomerCollection>> GetCustomers()
    {
        return ApiClient.CustomersAllAsync(null,null,null,null);
    }
}

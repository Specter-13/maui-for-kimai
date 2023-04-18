using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ApiClient.Interfaces;
public interface ICrudOperations<T,E>
{
    Task<T> Create(E entity);
    Task Delete(int id);
    Task<T> Read (int id);
    Task<T> Update(int id, E body);
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Interfaces;
public interface IServerService<T>
{
    Task<T> Create(T model);
    Task<T> Read(int id);
    Task<T> Update(T model);
    Task<bool> Delete(int id);
    Task<ICollection<T>> GetAll();
}

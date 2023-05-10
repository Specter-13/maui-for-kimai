using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Interfaces;
public interface ICrudOperationsMySql<T,O>
{
    Task<T> Create(O model);
    Task<T> Read(int id);
    Task<T> Update(O model);
    Task<bool> Delete(int id);
    Task<ICollection<T>> GetAll();
}

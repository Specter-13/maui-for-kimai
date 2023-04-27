using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Interfaces;
public interface IServerService /* : ICrudOperationsMySql<ServerEntity,ServerModel>*/
{
   Task<ServerEntity> GetDefaultServer();
   Task<ServerEntity> UnsetDefaultPropertyExceptOne(Guid id);

    Task<ServerEntity> Create(ServerModel model);
    Task<ServerEntity> Read(Guid id);
    Task<ServerEntity> Update(ServerModel model);
    Task<bool> Delete(Guid id);
    Task<ICollection<ServerEntity>> GetAll();
}

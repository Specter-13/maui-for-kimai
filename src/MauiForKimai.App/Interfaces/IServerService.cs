using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Interfaces;
public interface IServerService: ICrudOperationsMySql<ServerEntity,ServerModel>
{
   Task<ServerEntity> GetDefaultServer();
}

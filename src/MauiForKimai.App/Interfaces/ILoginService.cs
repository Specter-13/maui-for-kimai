
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Interfaces;

public interface ILoginService
{
    ApiLoginContext GetLoginContext();
    bool CheckIfConnected(ServerModel server);
  
    Task<bool> LoginOnStartUp(ServerModel defaultServer);

    Task<bool> Login(ServerModel server);
    Task Logout();

}

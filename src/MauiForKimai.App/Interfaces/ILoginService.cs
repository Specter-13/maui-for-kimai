
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Interfaces;

public interface ILoginService
{
    ApiStateProvider GetApiStateProvider();
    bool IsLogged();
    bool CheckIfConnected(ServerModel server);
    TimeSpan GetUserTimeOffset();
  
    Task<bool> TestConnection(ServerModel server);
    Task<bool> LoginToDefaultOnStartUp(ServerModel defaultServer);

    Task<bool> Login(ServerModel server);
    Task Logout();

}

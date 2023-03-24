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
    Task<bool> LoginToDefaultOnStartUp();
    Task Login();
    void Logout();

    Task TryToGetDefaultServer();
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Interfaces
{
    public interface ILoginService
    {
        Task<bool> LoginToDefaultOnStartUp();
        Task Login();
        Task Logout();

        Task TryToGetDefaultServer();
    }
}

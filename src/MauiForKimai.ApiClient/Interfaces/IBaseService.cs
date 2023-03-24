using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ApiClient.Interfaces;
public interface IBaseService
{
        bool IsClientInitialized();
        void InitializeClient(string baseUrl);
        void DeInitializeClient();
        Task<bool> PingServerAsync();
}

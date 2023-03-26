using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Interfaces;

public interface ISecureStorageService
{
    Task Save(string key, string value);
    Task<bool> Contains(string key);
    Task<String> Get(string key);
}

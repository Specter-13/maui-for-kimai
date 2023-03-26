using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Wrappers;
public interface IDispatcherWrapper
{
    //Task InvokeAsync(Action callback);
     IDispatcherTimer CreateTimer();
}

using Microsoft.Maui.Dispatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Wrappers;
public class DispatcherWrapper : IDispatcherWrapper
{
 
    private IDispatcher _dispatcher;

      public DispatcherWrapper(IDispatcher dispatcher)
      {
        if (dispatcher == null)
        {
          throw new ArgumentNullException(nameof(dispatcher));
        }
        _dispatcher = dispatcher;
      }



    public IDispatcherTimer CreateTimer()
    {
        return _dispatcher.CreateTimer();
    }
}

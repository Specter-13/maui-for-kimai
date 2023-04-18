using MauiForKimai.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Tests.Mocks;
public class FakeDispatcherWrapper : IDispatcherWrapper
{


    public IDispatcherTimer CreateTimer()
    {
        return new FakeDispatcherTimer();
    }
}

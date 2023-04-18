using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Tests.Mocks;
public class FakeDispatcherTimer : IDispatcherTimer
{
    public TimeSpan Interval { get; set; }
    public bool IsRepeating { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public bool IsRunning => throw new NotImplementedException();

    public event EventHandler Tick;

    public void Start()
    {
        return;
    }

    public void Stop()
    {
        return;
    }
}

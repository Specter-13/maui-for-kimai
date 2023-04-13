using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Wrappers;
public partial class TimerWrapper : ObservableObject
{
	private readonly IDispatcherWrapper _dispatcherWrapper;
    public TimerWrapper(IDispatcherWrapper dispatcher)
    {
		_dispatcherWrapper = dispatcher;
		CreateTimer();
        Time = new TimeSpan(0,0,0);
    }

	public void TimerStop()
	{ 
		_timer.Stop();
		_seconds = 0;
		Time = TimeSpan.FromSeconds(_seconds);
	}

	public void TimerStart()
	{ 
		_timer.Start();
		_seconds = 0;
	}

	public void TimerStartExisting(double duration)
	{ 
		_seconds = duration;
		Time = TimeSpan.FromSeconds(_seconds);
		_timer.Start();
		
	}
    private void CreateTimer()
	{ 
		//Application.Current.Dispatcher.C
		_timer = _dispatcherWrapper.CreateTimer();
		_timer.Interval = TimeSpan.FromSeconds(1);
		_timer.Tick += (s, e) =>
		{
			_seconds += 1;
			Time = TimeSpan.FromSeconds(_seconds);
		};
	}

	private void timer_Tick(object sender, EventArgs e)
	{
		_seconds += 1;
	}


    [ObservableProperty]
    public TimeSpan time;

    private double _seconds;
	private IDispatcherTimer _timer;
}

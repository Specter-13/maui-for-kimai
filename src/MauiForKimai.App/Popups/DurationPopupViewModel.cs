using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Popups;

public partial class DurationPopupViewModel : ObservableObject
{

    public DurationPopupViewModel(string duration)
    {
        var splitted = duration.Split(':');
		int hours;
		var resultHour = int.TryParse(splitted[0], out hours);
	    int minutes;
		var resultMinutes = int.TryParse(splitted[1], out minutes);

		if(resultHour && resultMinutes)
		{
			Hours = hours;
			Minutes = minutes;
		}
    }

    public bool IsNotValidMinutes => Validate();

    [ObservableProperty]
    public int hours;
    [ObservableProperty]
	[NotifyPropertyChangedFor(nameof(IsNotValidMinutes))]
	public int minutes;

	private bool Validate()
	{
		if(Minutes > 60 )
		{ 
			return true;
		}
		else
		{ 
			return false;
		}

	}
}

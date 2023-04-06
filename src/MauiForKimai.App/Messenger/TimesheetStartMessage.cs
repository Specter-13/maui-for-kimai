using CommunityToolkit.Mvvm.Messaging.Messages;
using MauiForKimai.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Messenger;

public class TimesheetStartNewMessage : ValueChangedMessage<TimesheetTimetrackingWrapper>
{
    public TimesheetStartNewMessage(TimesheetTimetrackingWrapper value) : base(value)
    {
    }
}

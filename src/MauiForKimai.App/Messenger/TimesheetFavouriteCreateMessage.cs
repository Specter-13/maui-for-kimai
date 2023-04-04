﻿using CommunityToolkit.Mvvm.Messaging.Messages;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Messenger;

public class TimesheetFavouriteCreateMessage : ValueChangedMessage<TimesheetListItemModel>
{
    public TimesheetFavouriteCreateMessage(TimesheetListItemModel value) : base(value)
    {
    }
}

﻿using CommunityToolkit.Mvvm.Messaging.Messages;
using MauiForKimai.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Messenger;

public class TimesheetActivityChooseMessage : ValueChangedMessage<ActivityListModel>
{
    public TimesheetActivityChooseMessage(ActivityListModel value) : base(value)
    {
    }
}
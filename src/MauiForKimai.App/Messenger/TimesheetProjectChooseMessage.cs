using CommunityToolkit.Mvvm.Messaging.Messages;
using MauiForKimai.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Messenger;

public class TimesheetProjectChooseMessage : ValueChangedMessage<ProjectListModel>
{
    public TimesheetProjectChooseMessage(ProjectListModel value) : base(value)
    {
    }
}

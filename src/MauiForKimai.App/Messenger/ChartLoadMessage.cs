using CommunityToolkit.Mvvm.Messaging.Messages;
using MauiForKimai.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.Messenger;

 public class ChartLoadMessage : ValueChangedMessage<string>
{
    public ChartLoadMessage(string value) : base(value)
    {
    }
}

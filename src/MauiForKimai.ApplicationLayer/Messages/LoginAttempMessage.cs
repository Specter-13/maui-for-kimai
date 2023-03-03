using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiForKimai.ApplicationLayer.Messages;


public class LoginAttemptMessage : ValueChangedMessage<string>
{
    public LoginAttemptMessage(string value) : base(value)
    {
    }
}


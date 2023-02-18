using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MauiForKimai.BL.Exceptions;
internal class NonExistingApiClientException : Exception
{
    public NonExistingApiClientException()
    {
    }
    public NonExistingApiClientException(string message)
        : base(message)
    {
    }

    
}

using System;
using System.Collections.Generic;
using System.Text;

namespace mail
{
    interface IMessageParser
    {
        string Parse(string message);
    }
}
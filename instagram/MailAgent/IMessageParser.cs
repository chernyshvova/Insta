using System;
using System.Collections.Generic;
using System.Text;

namespace MailAgent
{
    interface IMessageParser
    {
        string Parse(string message);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTools.MailAgent
{
    // TODO change to commons error handler 
    public class CustomException : Exception
    {
        public int m_code;
        public CustomException(int code)
        {
            m_code = code;
        }
    }
}

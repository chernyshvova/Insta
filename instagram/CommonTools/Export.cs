using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using mail;
using InstarmCore.Utils;

namespace CommonTools
{
        public class PyWrapper : DynamicObject
        {
            public bool enable = true;


            // ctor
            public PyWrapper()
            {
            }
            
            public int ParseALLMessage(string login, string password, string subject, string sender, out string result)
            {
                result = "";
                try
                {
                    mail.MailAgent agent = new mail.MailAgent(login, password);
                    result = agent.ParseMessage(subject, sender, new InstagrammVerifyParser());
                }
                catch(Exception)
                {
                    //...
                    
                }

                return Convert.ToInt32(ExeptionUtils.GetState());
        }

        }


}

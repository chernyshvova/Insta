using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using mail;

namespace CommonTools
{
        public class PyWrapper : DynamicObject
        {
            public bool enable = true;
            // wrapped C# class
            private mail.MailAgent functions;

            // ctor
            public PyWrapper(string login, string password)
            {

                try
                {
                    functions = new mail.MailAgent(login, password);
                }
                catch
                {
                    enable = false;
                    //error handle
                }
                
            }
            
            public int ParseALLMessage(string subject, string sender, out string result)
            {
                result = "";
                if(!enable)
                {
                    return 3;//E_INVALID_EMAIL
                }
                try
                {
                    DateTime messageTime = new DateTime(2019, 9, 28);
                    result = functions.ParseMessage(subject, sender, messageTime, new InstagrammVerifyParser());
                }
                catch(MailAgent.CustomException ex)
                {
                    return ex.m_code;
                }
                catch(Exception ex)
                {
                string err = ex.ToString();
                    //something wrong
                    return 99;
                }
            

                return 0; // 0 = S_OK
            }

        }


}

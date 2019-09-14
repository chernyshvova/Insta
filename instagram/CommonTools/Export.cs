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

            // wrapped C# class
            private mail.MailAgent functions;

            // ctor
            public PyWrapper()
            {
                functions = new mail.MailAgent("chernyshvova@gmail.com", "BamBam753951");
            }

            public string ParseMessage()
            {
                DateTime messageTime = new DateTime(2000, 1, 1);
                var code = functions.ParseMessage("Подтвердите свой аккаунт", "security@mail.instagram.com", messageTime, new InstagrammVerifyParser());
                return code;
            }

        }
}

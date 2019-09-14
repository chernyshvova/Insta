using System;
using System.Collections.Generic;

namespace MailAgent
{
    class Program
    {
        static void Main(string[] args)
        {
            MailAgent agent = new MailAgent("chernyshvova@gmail.com", "BamBam753951");
            DateTime messageTime = new DateTime(2000, 1, 1);
            string code = agent.ParseMessage("Подтвердите свой аккаунт", "security@mail.instagram.com", messageTime, new InstagrammVerifyParser());
            Console.WriteLine(code);
        }
    }
}

﻿using System;
using System.Collections.Generic;

namespace MailAgent
{
    class Program
    {
        static void Main(string[] args)
        {
            MailAgent agent = new MailAgent("chernyshvova@gmail.com", "BamBam753951");
            agent.ParseMessage("Подтвердите свой аккаунт", "security@mail.instagram.com", new InstagrammVerifyParser());
        }
    }
}

using Limilabs.Client.IMAP;
using Limilabs.Client.POP3;
using Limilabs.Mail;
using System;
using System.Collections.Generic;

namespace MailAgent
{
    class Program
    {
        static void Receive()
        {
                using (Imap imap = new Imap())
                {
                    imap.ConnectSSL("imap.gmail.com", 993);  // or ConnectSSL for SSL
                    imap.UseBestLogin("chernyshvova", "BamBam753951");
                    imap.SelectInbox();
                    List<long> uids = imap.Search(Flag.Unseen);
                    foreach (long uid in uids)
                    {
                        IMail email = new MailBuilder()
                            .CreateFromEml(imap.GetMessageByUID(uid));
                        Console.WriteLine(email.Text);
                    }
                    imap.Close();
                }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Receive();
        }
    }
}

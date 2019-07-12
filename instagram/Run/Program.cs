using System;
using MailUtils;
namespace Run
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            MailUtils.MailUtils email = new MailUtils.MailUtils();
            email.GetMail();
        }
    }
}

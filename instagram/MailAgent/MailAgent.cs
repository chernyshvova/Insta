using Limilabs.Client.IMAP;
using Limilabs.Mail;
using System;
using System.Collections.Generic;
using System.Text;


/// <summary>
/// https://myaccount.google.com/lesssecureapps
/// </summary>
namespace MailAgent
{
    class MailAgent
    {
        public MailAgent(string login, string password)
        {
            ParseCredentials(login);
            ResolvePort();
            this.m_password = password;
            this.m_smtpAgent = new Imap();

            if (this.m_ssl)
            {
                this.m_smtpAgent.ConnectSSL(this.m_host, this.m_port);
            }
            else
            {
                this.m_smtpAgent.Connect(this.m_host, this.m_port);
            }

        }

        public string ParseInsTagrammVify()
        {
            const string subject = "Verify Your Account";
            List<long> uids = this.m_smtpAgent.Search(Flag.New);

            foreach (long uid in uids)
            {
                IMail email = new MailBuilder()
                    .CreateFromEml(this.m_smtpAgent.GetMessageByUID(uid));

                if(email.Subject == subject)
                {
                    Console.WriteLine(email.Text);
                }
                return email.Text;
            }
            return "Fuck off";
        }
        private void ParseCredentials(string login)
        {
            const int hostIndex = 1;
            this.m_host = login.Split("@")[hostIndex];
        }

        private void ResolvePort()
        {
            switch(this.m_host)
            {
                case "gmail.com":
                    this.SetParsedData(993, true);
                    break;
                default:
                    throw new Exception("Unknown parsed host");
            }
        }

        private void SetParsedData(int port, bool ssl)
        {
            if(ssl)
            {
                this.m_ssl = ssl;
            }
            this.m_port = port;
        }

        private string m_login;
        private string m_password;
        private string m_host;
        private int m_port;
        private Imap m_smtpAgent;
        private bool m_ssl;


    }
}

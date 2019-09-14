using AE.Net.Mail;
using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// https://myaccount.google.com/lesssecureapps
/// https://github.com/andyedinborough/aenetmail
/// </summary>
namespace MailAgent
{
    enum Language
    {
        Rus,
        Eng
    }

    class MailAgent
    {
        public MailAgent(string login, string password)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            const int hostIndex = 1;
            string serverName = login.Split('@')[hostIndex];
            ResolvePort(serverName);
            m_client = new AE.Net.Mail.ImapClient(m_host, login, password, AE.Net.Mail.AuthMethods.Login, m_port, m_ssl, true);

        }

        public string ParseMessage(string subj, string sender, IMessageParser parser)
        {
            SearchCondition condition = new SearchCondition();
            condition.Value = string.Format(@"X-GM-RAW ""AFTER:{0:yyyy-MM-dd}""", new DateTime(2000, 1, 1));
            var msgs = m_client.SearchMessages(condition);

            List<Lazy<MailMessage>> resultMessages = new List<Lazy<MailMessage>>();
            foreach(var msg in msgs)
            {
                if (msg.Value.Subject == subj && msg.Value.From.Address == sender)
                {
                    resultMessages.Add(msg);
                }
            }

            resultMessages.Sort((x, y) =>
            x.Value.Date.CompareTo(y.Value.Date)
            );

            if(resultMessages.Count == 0)
            {
                throw new Exception("message is not found");
            }

            return parser.Parse(resultMessages[resultMessages.Count].Value.Body);
            
        }

        private void ResolvePort(string serverName)
        {
            switch(serverName)
            {
                case "gmail.com":
                    this.SetParsedData(993, true, "imap.gmail.com");
                    break;
                default:
                    throw new Exception("Unknown parsed host");
            }
        }

        private void SetParsedData(int port, bool ssl, string host)
        {
            m_ssl = ssl;
            m_port = port;
            this.m_host = host;
        }

        private string m_host;
        private int m_port;
        AE.Net.Mail.ImapClient m_client;
        private bool m_ssl;


    }
}

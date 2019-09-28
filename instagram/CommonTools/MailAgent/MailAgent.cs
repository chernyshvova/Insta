using AE.Net.Mail;
using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// https://myaccount.google.com/lesssecureapps
/// https://github.com/andyedinborough/aenetmail
/// </summary>
namespace mail
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

        public string ParseMessage(string subj, string sender, DateTime dateFrom, IMessageParser parser)
        {
            SearchCondition condition = new SearchCondition();
            if (this.m_host == "gmail.com")
            {
                condition.Value = string.Format(@"X-GM-RAW ""AFTER:{0:yyyy-MM-dd}""", dateFrom);
            }
            else
            {
                condition = SearchCondition.Undeleted();
            }
            
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
                throw new CommonTools.MailAgent.CustomException(1);//E_DATA_NOT_FOUND
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
                case "seznam.cz":
                    this.SetParsedData(993, true, "imap.seznam.cz");
                    break;
                default:
                    throw new CommonTools.MailAgent.CustomException(2); //2 = MAIL_PROVIDER_NOT_FOUND
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

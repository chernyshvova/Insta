using AE.Net.Mail;
using System;
using System.Collections.Generic;
using System.Text;
using InstarmCore.Utils;

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

    enum EMAIL_HRESULT
    {
        INVALID_CREDENTIALS	 = -2146233088
    }

    class MailAgent
    {
        public MailAgent(string login, string password)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            const int hostIndex = 1;
            string serverName = login.Split('@')[hostIndex];
            ResolvePort(serverName);
            try
            {
                m_client = new AE.Net.Mail.ImapClient(m_host, login, password, AE.Net.Mail.AuthMethods.Login, m_port, m_ssl, true);
            }
            catch (Exception ex)
            {
                if(ex.HResult == (int)EMAIL_HRESULT.INVALID_CREDENTIALS)
                {
                    ExceptionUtils.SetState(Error.E_INVALID_EMAIL, ex.Message);
                }
                
            }
        }

        public string ParseMessage(string subj, string sender, IMessageParser parser)
        {
            SearchCondition condition = new SearchCondition();
            if (this.m_host == "gmail.com")
            {
                DateTime messageTime = new DateTime(2000, 9, 28);
                condition.Value = string.Format(@"X-GM-RAW ""AFTER:{0:yyyy-MM-dd}""", messageTime);
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
                ExceptionUtils.SetState(Error.E_DATA_NOT_FOUND);
                return "";
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
                    ExceptionUtils.Throw(Error.E_EMAIL_PROVIDER_NOT_FOUND, "this email server is not unsupported");
                    break;
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

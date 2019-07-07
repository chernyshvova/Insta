using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace instarm
{
    public class ProxyData
    {
        public string host;
        public string port;
        public string username;
        public string password;

        public ProxyData(string proxyHost, string proxyPort, string proxyUsername, string proxyPassword)
        {
            this.host = proxyHost;
            this.port = proxyPort;
            this.username = proxyUsername;
            this.password = proxyPassword;
        }



    }
}

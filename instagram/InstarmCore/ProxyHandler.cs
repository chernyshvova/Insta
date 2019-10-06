using System;
using System.Net;
using System.Net.Http;


namespace InstarmCore
{
    class ProxyHandler
    {
        public HttpClientHandler getProxy(ProxyData data) {
          //  string host, string port, string proxyUserName, string proxyPassword
            WebProxy proxy;
            if (data.username != null)
            {
                proxy = new WebProxy()
                {
                    Address = new Uri("http://" + data.host + ":" + data.port), //i.e: http://1.2.3.4.5:8080
                    BypassProxyOnLocal = false,
                    UseDefaultCredentials = false,
                    // *** These creds are given to the proxy server, not the web server ***              
                    Credentials = new NetworkCredential(
                    userName: data.username,
                    password: data.password)
                };
            }
            else
            {
                proxy = new WebProxy()
                {
                    Address = new Uri("http://" + data.host + ":" + data.port), //i.e: http://1.2.3.4.5:8080
                    BypassProxyOnLocal = false,
                    UseDefaultCredentials = false,
                };
            }


            // Now create a client handler which uses that proxy
            var httpClientHandler = new HttpClientHandler()
            {
                Proxy = proxy,
            };

            bool needServerAuthentication = false;
            // Omit this part if you don't need to authenticate with the web server:
            if (needServerAuthentication)
            {
                httpClientHandler.PreAuthenticate = true;
                httpClientHandler.UseDefaultCredentials = false;

                // *** These creds are given to the web server, not the proxy server ***
                httpClientHandler.Credentials = new NetworkCredential(
                    userName: "serverUserName",
                    password: "serverPassword");
            }
            return httpClientHandler;
        }
       
}
}

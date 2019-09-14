using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mail
{
    class InstagrammVerifyParser : IMessageParser
    {
        public string Parse(string message)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(message);


            foreach (var node in doc.DocumentNode.SelectNodes("//p[@style]"))
            {

                if (node.InnerText.All(char.IsDigit))
                {
                    return node.InnerText;
                }
            }

            throw new Exception("failed to parse message");
        }
    }
}

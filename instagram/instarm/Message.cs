using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace instarm
{
    public class Message
    {
        public string id;
        public string threadId;
        public string sender;
        public string reciver;
        public string message;
        public DateTime timestamp;

        public Message(string sender, string resiver, string threadId, string id, string message, DateTime timestamp)
        {
            this.id = id;
            this.threadId = threadId;
            this.sender = sender;
            this.reciver = resiver;
            this.message = message;
            this.timestamp = timestamp;
        }
    }
}

using InstarmCore.Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace InstarmCore
{
    class Messages
    {
        List<Message> messages = new List<Message>();
        List<Message> newMessages = new List<Message>();
        JsonSerializer serializer = new JsonSerializer();
        public string accountName;
        private string path = "";
        
        bool readed = false;

        public Messages(string accountName)
        {
            this.accountName = accountName;
            path = Environment.CurrentDirectory + PathContract.pathAccount + accountName + @"\messages.txt";
        }

        public void StackMessage(string sender, string resiverId, string threadId, string id, string message, DateTime timestamp)
        {
            if (!FindMessage(id))
            {
                if (threadId.Equals(resiverId))
                {
                    resiverId = sender;
                    sender = accountName;                 
                }
                else
                {
                    resiverId = accountName;
                }
                Message msg = new Message(sender, resiverId, threadId, id, message, timestamp);
                newMessages.Add(msg);
            }
        }
        public void WriteMessages()
        {
            foreach (var item in newMessages.AsEnumerable().Reverse())
            {
                messages.Insert(0,item);
            }
            using (StreamWriter sw = new StreamWriter(path))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, messages);
            }         
            using (FileStream fstream = new FileStream(Environment.CurrentDirectory + PathContract.pathNew, FileMode.OpenOrCreate)) {
                string incoming = "";
                foreach (var item in newMessages)
                {
                    incoming += "Отправитель: " + item.sender + "   Получатель: " + item.reciver + "    Cообщение: " + item.message + Environment.NewLine;
                }
                fstream.SetLength(0);
                byte[] array = System.Text.Encoding.Unicode.GetBytes(incoming);
                fstream.Write(array, 0, array.Length);
            }
            IDbHelper db = new DbHelperSQLite();
            db.writeMessages(newMessages);
            messages.Clear();
            newMessages.Clear();
        }

        public void ReadMessages()
        {
            if (File.Exists(path))
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    var msgs = (List<Message>)serializer.Deserialize(sr, typeof(List<Message>));
                    if (msgs != null)
                    {
                        messages = msgs;
                    }
                }
            }
        }

        private bool FindMessage(string messageId)
        {
            if (!readed)
            {
                ReadMessages();
                readed = true;
            }
            
            if (messages.Any())
            {
                foreach (var item in messages)
                {
                    if (item.id.Equals(messageId))
                    {
                        return true;
                    }
                }
            }       
            return false;
        }
    }
}

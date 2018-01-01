using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Game.Message
{
    public class MessageManager
    {
        private Queue<Message> msgQueue;
        private static MessageManager instance;
        public static MessageManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MessageManager();
                }
                return instance;
            }
        }

        public MessageManager()
        {
            msgQueue = new Queue<Message>();
        }

        public void SendMessage(double delay,int sender,int receiver,object extraInfo)
        {
            Message msg = new Message(delay, sender, receiver, extraInfo);
            msgQueue.Enqueue(msg);
        }
    }
}

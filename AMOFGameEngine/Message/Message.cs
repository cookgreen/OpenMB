using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Game.Message
{
    public class Message
    {
        private double delay;
        private int sender;
        private int receiver;
        private object extraInfo;

        public object ExtraInfo
        {
            get { return extraInfo; }
        }

        public int Receiver
        {
            get { return receiver; }
        }

        public int Sender
        {
            get { return sender; }
        }

        public double Delay
        {
            get { return delay; }
        }

        public Message(double delay,
                       int sender,
                       int receiver,
                       object extraInfo)
        {
            this.delay = delay;
            this.sender = sender;
            this.receiver = receiver;
            this.extraInfo = extraInfo;
        }
    }
}

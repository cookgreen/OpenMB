using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace AMOFGameEngine.Dedicated
{
    public class SendData
    {
        public Vector3 position;//position of the player
        public byte[] buffer;
        public int Length
        {
            get
            {
                return buffer.Length;
            }
        }
    }
}

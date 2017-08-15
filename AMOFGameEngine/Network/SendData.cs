using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MOIS;

namespace AMOFGameEngine.Network
{
    public class SendData
    {
        public string optType { get; set; }//Operation Type
        public KeyCode key { get; set; }//Pressed Key
        public MouseButtonID mouseBtn { get; set; }//Pressed Mouse Button
    }
}

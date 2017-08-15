using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Network
{
    public class ServerMetaData
    {
        public string Name { get; set; }
        public string Port { get; set; }
        public Dictionary<string, string> Options { get; set; }
    }
}

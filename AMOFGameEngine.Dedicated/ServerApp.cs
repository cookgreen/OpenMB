using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace AMOFGameEngine.Dedicated
{
    public class ServerApp
    {
        string name;
        int port;
        Server s;

        public ServerApp()
        {

            s = new Server(name, port);
        }
        public void Run()
        {
            if (!s.Start())
            {
                return;
            }
        }
    }
}

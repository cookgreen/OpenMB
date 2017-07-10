using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Dedicated
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerApp server = new ServerApp();
            server.Run();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Mogre;
using AMOFGameEngine.RPG;

namespace AMOFGameEngine.Dedicated
{
    public class Server
    {
        LogManager log;
        TcpListener listener;
        Dictionary<uint, Character> agents = new Dictionary<uint, Character>();
        const int MAX_PLAYER = 25;

        string serverName;
        int serverPort;

        public Server(string name,int port)
        {
            serverName = name;
            serverPort = port;
            listener = new TcpListener(IPAddress.Any, serverPort);
        }

        public bool Start()
        {
            try
            {
                listener.Start(MAX_PLAYER);
            }
            catch(Exception ex)
            {
                LogManager.Singleton.LogMessage(ex.Message);
            }

            return true;
        }
    }
}

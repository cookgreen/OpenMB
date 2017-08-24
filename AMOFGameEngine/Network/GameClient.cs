using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace AMOFGameEngine.Network
{
    public class GameClient
    {
        private TcpClient clientSock;
        private string username;
        private int port;

        public GameClient(int port,string username)
        {
            clientSock = new TcpClient(new IPEndPoint(new IPAddress(new byte[] { 127, 0, 0, 1 }), port));
            this.port = port;
            this.username = username;
        }

        public void EstablishConnectionToServer(string serverip)
        {
            try
            {
                clientSock.Connect(new IPAddress(Encoding.GetEncoding("UTF-8").GetBytes(serverip)), port);
                if (!clientSock.Connected)
                {
                    return;
                }
                using (BinaryWriter bw = new BinaryWriter(clientSock.GetStream()))
                {
                    bw.Write(username);
                }
            }
            catch
            {
                return;
            }
        }
    }
}

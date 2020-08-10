using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.DedicatedServer
{
	public class ServerApp
	{
		private int port;
		private TcpListener listener;
		private List<GameClient> clients;
		public ServerApp(int port)
		{
			this.port = port;
			listener = new TcpListener(IPAddress.Any, port);
			listener.Start();
			clients = new List<GameClient>();
		}

		public void Go()
		{
			Console.WriteLine("OpenMB Server run on port: " + port.ToString());

			while (true)
			{
				var tcpClient = listener.AcceptTcpClient();
				GameClient gameClient = new GameClient(tcpClient);
				clients.Add(gameClient);
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.DedicatedServer
{
	public class GameClient
	{
		private TcpClient client;
		public GameClient(TcpClient client)
		{
			this.client = client;
		}
	}
}

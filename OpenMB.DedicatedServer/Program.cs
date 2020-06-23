using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.DedicatedServer
{
	class Program
	{
		static void Main(string[] args)
		{
			int port = 8890;
			if (args.Length > 0)
			{
				int.TryParse(args[1], out port);
			}
			ServerApp server = new ServerApp(8890);
			server.Go();
		}
	}
}

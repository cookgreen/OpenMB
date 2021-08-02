using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Core
{
	public class GameRegisterManager
	{
		private Dictionary<string, object> registers;
		private static GameRegisterManager instance;
		public static GameRegisterManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new GameRegisterManager();
				}
				return instance;
			}
		}

		public GameRegisterManager()
		{
			registers = new Dictionary<string, object>();
			for (int i = 0; i < 15; i++)
			{
				registers.Add("reg" + i.ToString(), "UNRECONIZED TOKEN");
				registers.Add("s" + i.ToString(), "UNRECONIZED TOKEN");
			}
		}

		public object GetRegisterValue(string registerName)
		{
			if (registers.ContainsKey(registerName))
			{
				return registers[registerName];
			}
			else
			{
				return "UNRECONIZED REGISTER";
			}
		}

		public void SetRegisterValue(string registerName, object registerValue)
		{
			if (registers.ContainsKey(registerName))
			{
				registers[registerName] = registerValue;
			}
		}
	}
}

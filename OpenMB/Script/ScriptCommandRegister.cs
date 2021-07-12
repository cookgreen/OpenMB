using OpenMB.Script.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Script
{
	class ScriptCommandRegister
	{
		private Dictionary<string, Type> registerCommand;
		public Dictionary<string, Type> RegisteredCommand
		{
			get
			{
				return registerCommand;
			}
		}
		private static ScriptCommandRegister instance;
		public static ScriptCommandRegister Instance
		{
			get
			{
				if (instance == null)
					instance = new ScriptCommandRegister();
				return instance;
			}
		}

		public ScriptCommandRegister()
		{
			registerCommand = new Dictionary<string, Type>();
		}

		public void RegisterNewCommand(string commandName, Type type)
		{
			if (!RegisteredCommand.ContainsKey(commandName))
			{
				if (!RegisteredCommand.ContainsValue(type))
				{
					registerCommand.Add(commandName, type);
				}
				else
				{
					EngineManager.Instance.log.LogMessage(string.Format("The type with name `{0}` has been already registered into the engine!", type.FullName), LogMessage.LogType.Warning);
				}
			}
			else
			{
				EngineManager.Instance.log.LogMessage(string.Format("The command with name `{0}` has been already registered into the engine!", commandName), LogMessage.LogType.Warning);
			}
		}
	}
}

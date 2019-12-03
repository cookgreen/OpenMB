using OpenMB.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script.Command
{
	class ChangeSystemVariableValue : ScriptCommand
	{
		private string[] commandArgs;
		public ChangeSystemVariableValue()
		{
			commandArgs = new string[3] {
				"variable name",
				"variable member",
				"value"
			};
		}
		public override string[] CommandArgs
		{
			get
			{
				return commandArgs;
			}
		}

		public override string CommandName
		{
			get
			{
				return "change_variable_value";
			}
		}

		public override ScriptCommandType CommandType
		{
			get
			{
				return ScriptCommandType.Line;
			}
		}

		public override void Execute(params object[] executeArgs)
		{
			if (CommandArgs.Length == 3)
			{
				GameWorld world = executeArgs[0] as GameWorld;
				string variableName = getParamterValue(CommandArgs[0], world);
				string variableMember = getParamterValue(CommandArgs[1], world);
				string destVar = getParamterValue(CommandArgs[2], world);
				if (world.GlobalVariableTable.ContainsKey(variableName))
				{
					object obj = world.GlobalVariableTable[variableName];
					var objProperty = obj.GetType().GetProperty(variableMember);
					if (objProperty != null)
					{
						objProperty.SetValue(obj, destVar);
					}
				}
			}
		}
	}
}

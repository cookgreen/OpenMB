using OpenMB.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Script.Command
{
	class InitVectorScriptCommand : ScriptCommand
	{
		public InitVectorScriptCommand()
		{
			commandArgs = new string[] {
				"Vector"
			};
		}
		private string[] commandArgs;
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
				return "init_vector";
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
			GameWorld world = executeArgs[0] as GameWorld;
			string vectorVariable = CommandArgs[0].ToString();

			ScriptLinkTableNode vector = world.GlobalValueTable.GetRecord(vectorVariable);
			if (vector != null)
			{
				vector.NextNodes[0].Value = "0";
				vector.NextNodes[1].Value = "0";
				vector.NextNodes[2].Value = "0";
			}
		}
	}
}

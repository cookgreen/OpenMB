using OpenMB.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Script.Command
{
	public class ChangeHitpointScriptCommand : ScriptCommand
	{
		private string[] commandArgs;

		public override string CommandName
		{
			get
			{
				return "change_hitpoint";
			}
		}

		public override string[] CommandArgs
		{
			get
			{
				return commandArgs;
			}
		}

		public override ScriptCommandType CommandType
		{
			get
			{
				return ScriptCommandType.Line;
			}
		}

		public ChangeHitpointScriptCommand()
		{
			commandArgs = new string[] {
				"EffecterId",
				"BeEffectedId",
				"value"
			};
		}

		public override void Execute(params object[] executeArgs)
		{
			GameWorld world = executeArgs[0] as GameWorld;
			var gameObject = world.GetAgentById(int.Parse(getVariableValue(commandArgs[1]).ToString()));
			if (gameObject == null)
			{
				return;
			}
			gameObject.Health.EffectHealth(
				int.Parse(getVariableValue(commandArgs[0]).ToString()),
				int.Parse(getVariableValue(commandArgs[2]).ToString()));
		}
	}
}

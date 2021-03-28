using OpenMB.Game;
using OpenMB.Screen;
using OpenMB.UI;
using OpenMB.UI.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script.Command
{
	public class WidgetGetTextScriptCommand : ScriptCommand
	{
		private string[] commandArgs;
		public override string CommandName
		{
			get { return "widget_get_text"; }
		}

		public override ScriptCommandType CommandType
		{
			get { return ScriptCommandType.Line; }
		}

		public override string[] CommandArgs
		{
			get { return commandArgs; }
		}

		public WidgetGetTextScriptCommand()
		{
			commandArgs = new string[]
			{
				"dest value",
				"widget name"
			};
		}

		public override void Execute(params object[] executeArgs)
		{
			GameWorld world = executeArgs[0] as GameWorld;
			IScreen screen = executeArgs[1] as IScreen;

			string variable = commandArgs[0];
			var w = screen.UIWidgets.Where(o => o.Name == getVariableValue(commandArgs[1]).ToString()).FirstOrDefault();
			if ((w as TextWidget) != null)
			{
				string value = (w as TextWidget).Text;
				if (variable.StartsWith("%"))
				{
					Context.ChangeLocalValue(variable.Substring(1), value);
				}
				else if (variable.StartsWith("$"))
				{
					world.ChangeGobalValue(variable.Substring(1), value);
				}
			}
		}
	}
}

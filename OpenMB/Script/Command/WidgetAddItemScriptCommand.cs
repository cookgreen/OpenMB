using OpenMB.Game;
using OpenMB.Screen;
using OpenMB.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script.Command
{
    public class WidgetAddItemScriptCommand : ScriptCommand
    {
        private string[] commandArgs;
        public override string CommandName
        {
            get { return "widget_add_item"; }
        }

        public override ScriptCommandType CommandType
        {
            get { return ScriptCommandType.Line; }
        }

        public override string[] CommandArgs
        {
            get { return commandArgs; }
        }

        public WidgetAddItemScriptCommand()
        {
            commandArgs = new string[]
            {
                "widget name",
                "item string"
            };
        }

        public override void Execute(params object[] executeArgs)
        {
            var world = executeArgs[0] as GameWorld;
            var currentScreen = executeArgs[1] as IScreen;

            string widgetName = getParamterValue(commandArgs[0], world);
            var w = currentScreen.UIWidgets.Where(o => o.Name == widgetName).FirstOrDefault();
			if (w != null && (w as IHasSubItems) != null)
			{
				(w as IHasSubItems).AddItem(getParamterValue(commandArgs[1], world));
			}
        }
    }
}

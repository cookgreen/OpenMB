using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script.Command
{
    public class ConditionalControlScriptCommand : ScriptCommand
	{
		private string[] commandArgs;
        private ConditionalTree conditionalTree;

        public override string CommandName
		{
			get { return "if_cond"; }
		}

		public override string[] CommandArgs
		{
			get { return commandArgs; }
		}

		public override ScriptCommandType CommandType
		{
			get { return ScriptCommandType.ConditionControl; }
		}

		public ConditionalControlScriptCommand()
		{
			commandArgs = new string[]
			{
			};
		}

        public override void Execute(params object[] executeArgs)
		{
			conditionalTree = new ConditionalTree();
			ConditionalTreeNode node = new ConditionalTreeNode();
			conditionalTree.RootNode = node;

			IScriptCommand lastCommand = null;

			foreach (var subCommand in SubCommands)
			{
				if (subCommand is ConditionalControlTurningScriptCommand)
					continue;

				if (subCommand is ConditionalCheckScriptCommand)
				{
					if (!(lastCommand is ConditionalCheckScriptCommand))
					{
						ConditionalTreeNode newNode = new ConditionalTreeNode();
						newNode.Statements.Add(subCommand);
						node.SubNodes.Add(newNode);
						node = newNode;
					}
					else
					{
						node.Conditions.Add(subCommand);
					}
				}
				else
				{
					node.Statements.Add(subCommand);
				}

				lastCommand = subCommand;

			}

			conditionalTree.Execute(executeArgs);
		}
    }
}

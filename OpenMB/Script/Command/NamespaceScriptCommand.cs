using OpenMB.Game;
using Mogre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Script.Command
{
    class NamespaceScriptCommand : ScriptCommand
    {
        private string[] commandArgs;
        public override string CommandName
        {
            get
            {
                return "namespace";
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

        public NamespaceScriptCommand()
        {
            commandArgs = new string[] {
                "namespace name"
            };
        }

        public override void Execute(params object[] executeArgs)
        {
            string namespaceName = commandArgs[0];
            if (executeArgs.Count() == 1 && executeArgs[0] is ScriptPreprocessor)
            {
                ScriptPreprocessor preprocessor = executeArgs[0] as ScriptPreprocessor;
                preprocessor.Add(namespaceName, Context.File);
            }
        }
    }
}

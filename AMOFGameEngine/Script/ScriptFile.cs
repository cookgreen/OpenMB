using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Script
{
    public class ScriptFile
    {
        public string FileName { get; set; }
        public Queue<IScriptCommand> ScriptCommands { get; set; }
        public ScriptFile()
        {
            ScriptCommands = new Queue<IScriptCommand>();
        }
        public void Execute(params object[] executeParams)
        {
            while(ScriptCommands.Count >0)
            {
                ScriptCommands.Dequeue().Execute(executeParams);
            }
        }
    }
}

using Mogre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMOFGameEngine.Script.Command;

namespace AMOFGameEngine.Script
{
    public class ScriptFile
    {
        private Dictionary<string, IScriptCommand> registeredCommand;
        public ScriptContext Context { get; set; }
        public string FileName { get; set; }
        public Queue<IScriptCommand> ScriptCommands { get; set; }
        public ScriptFile()
        {
            ScriptCommands = new Queue<IScriptCommand>();
            Context = new ScriptContext();

            registeredCommand = new Dictionary<string, IScriptCommand>();
            IScriptCommand spawnCommand = new SpawnScriptCommand();
            IScriptCommand teamCommand = new TeamScriptCommand();
            IScriptCommand assignCommand = new AssignScriptCommand(Context);
            registeredCommand.Add(spawnCommand.CommandName, spawnCommand);
            registeredCommand.Add(teamCommand.CommandName, teamCommand);
        }
        public void Execute(params object[] executeParams)
        {
            while(ScriptCommands.Count >0)
            {
                ScriptCommands.Dequeue().Execute(executeParams);
            }
        }

        internal void Parse(string groupName, params object[] runArgs)
        {
            DataStreamPtr dataStream = ResourceGroupManager.Singleton.OpenResource(FileName, groupName);
            string dataString = dataStream.AsString;

            string[] lines = dataString.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lines.Length; i++)
            {
                string[] lineToken = lines[i].Split(' ');
                if (registeredCommand.ContainsKey(lineToken[0]))
                {
                    for (int j = 1; j < lineToken.Length; j++)
                    {
                        registeredCommand[lineToken[0]].PushArg(lineToken[j], j - 1);
                    }
                    ScriptCommands.Enqueue(registeredCommand[lineToken[0]]);
                }
            }
            Execute(runArgs);
        }
    }
}

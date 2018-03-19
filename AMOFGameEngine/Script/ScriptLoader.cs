using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using AMOFGameEngine.Game;
using Mogre;

namespace AMOFGameEngine.Script
{
    public class ScriptLoader
    {
        private Dictionary<string, IScriptCommand> registeredCommand;
        public ScriptLoader()
        {
            registeredCommand = new Dictionary<string, IScriptCommand>();
            IScriptCommand spawnCommand = new SpawnScriptCommand();
            registeredCommand.Add(spawnCommand.CommandName, spawnCommand);
        }
        public void Parse(string scriptFileName, string groupName, params object[] runArgs)
        {
            DataStreamPtr dataStream = ResourceGroupManager.Singleton.OpenResource(scriptFileName, groupName);
            string dataString = dataStream.AsString;

            ScriptFile file = new ScriptFile();
            file.FileName = scriptFileName;
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
                    file.ScriptCommands.Enqueue(registeredCommand[lineToken[0]]);
                }
            }
            file.Execute(runArgs);
        }
    }
}

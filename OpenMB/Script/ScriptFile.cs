using Mogre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using OpenMB.Script.Command;

namespace OpenMB.Script
{
    public class ScriptFile
    {
        private Stack<IScriptCommand> tempCommandStack;
        private Dictionary<string, Type> registeredCommand;
        private RootScriptCommand root;
        public ScriptContext Context { get; set; }
        public string FileName { get; set; }
        public List<IScriptCommand> Commands
        {
            get
            {
                return root.SubCommands;
            }
        }

        public ScriptFile()
        {
            Context = new ScriptContext(this);
            root = new RootScriptCommand();

            tempCommandStack = new Stack<IScriptCommand>();

            registeredCommand = ScriptCommandRegister.Instance.RegisteredCommand;
        }

        public ScriptFunction FindFunction(string functionName)
        {
            return Context.GetFunction(functionName);
        }
        public void Execute(params object[] executeParams)
        {
            root.Execute(executeParams);
        }

        public void Parse(string groupName, params object[] runArgs)
        {
            ScriptCommand currentCommand = null;
            currentCommand = root;
            DataStreamPtr dataStream = ResourceGroupManager.Singleton.OpenResource(FileName, groupName);
            string dataString = dataStream.AsString;

            string[] lines = dataString.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            int length = lines.Length;
            for (int i = 0; i < length; i++)
            {
                lines[i] = lines[i].Replace("\t", null);
                if (string.IsNullOrEmpty(lines[i]))
                {
                    continue;
                }
                if (lines[i].StartsWith("#")) //skip comments
                {
                    continue;
                }
                string[] lineToken = lines[i].Split(' ');
                if (lineToken.Length <= 0)
                {
                    GameManager.Instance.log.LogMessage("Error Prase Script File At Line: '" + lineToken[0] + "' Error At Line: " + (i + 1).ToString(), LogMessage.LogType.Error);
                    continue;
                }
                if (!registeredCommand.ContainsKey(lineToken[0]))
                {
                    GameManager.Instance.log.LogMessage("Script Command '" + lineToken[0] + "' Not Found At Line: " + (i + 1).ToString(), LogMessage.LogType.Warning);
                    continue;
                }
                try
                {
                    ScriptCommand scriptCommand;
                    scriptCommand = Activator.CreateInstance(registeredCommand[lineToken[0]]) as ScriptCommand;
                    scriptCommand.ParentCommand = currentCommand;
                    scriptCommand.Context = Context;
                    int tokenLength = lineToken.Length;
                    for (int j = 1; j < tokenLength; j++)
                    {
                        scriptCommand.PushArg(lineToken[j], j - 1);
                    }
                    switch (scriptCommand.CommandType)
                    {
                        case ScriptCommandType.Line:
                            currentCommand.SubCommands.Add(scriptCommand);
                            break;
                        case ScriptCommandType.Block:
                            currentCommand.SubCommands.Add(scriptCommand);
                            currentCommand = scriptCommand;
                            break;
                        case ScriptCommandType.End:
                            switch (currentCommand.CommandName)
                            {
                                case "function":
                                    Context.RegisterFunction(currentCommand.CommandArgs[0], currentCommand.SubCommands);
                                    break;
                            }
                            currentCommand = currentCommand.ParentCommand;
                            break;
                    }
                }
                catch
                {
                    currentCommand = null;
                    GameManager.Instance.log.LogMessage("Script Command '" + lineToken[0] + "' Error At Line: " + (i + 1).ToString(), LogMessage.LogType.Error);
                    continue;
                }
            }
        }
        public IScriptCommand ParseOneLine(string groupName, int lineNo = 1)
        {
            if (lineNo < 1)
            {
                GameManager.Instance.log.LogMessage("Invalid Line number!", LogMessage.LogType.Error);
                return null;
            }

            ScriptCommand currentCommand = null;
            currentCommand = root;
            DataStreamPtr dataStream = ResourceGroupManager.Singleton.OpenResource(FileName, groupName);
            string dataString = dataStream.AsString;

            string[] lines = dataString.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            int length = lines.Length;
            for (int i = 0; i < length; i++)
            {
                if (i == lineNo - 1)
                {
                    lines[i] = lines[i].Replace("\t", null);
                    if (string.IsNullOrEmpty(lines[i]))
                    {
                        continue;
                    }
                    string[] lineToken = lines[i].Split(' ');
                    if (lineToken.Length <= 0)
                    {
                        GameManager.Instance.log.LogMessage("Error Prase Script File At Line: '" + lineToken[0] + "' Error At Line: " + (i + 1).ToString(), LogMessage.LogType.Error);
                        continue;
                    }
                    if (!registeredCommand.ContainsKey(lineToken[0]))
                    {
                        GameManager.Instance.log.LogMessage("Script Command '" + lineToken[0] + "' Not Found At Line: " + lineNo.ToString(), LogMessage.LogType.Warning);
                        continue;
                    }
                    try
                    {
                        ScriptCommand scriptCommand;
                        scriptCommand = Activator.CreateInstance(registeredCommand[lineToken[0]]) as ScriptCommand;
                        scriptCommand.ParentCommand = currentCommand;
                        scriptCommand.Context = Context;
                        int tokenLength = lineToken.Length;
                        for (int j = 1; j < tokenLength; j++)
                        {
                            scriptCommand.PushArg(lineToken[j], j - 1);
                        }
                        switch (scriptCommand.CommandType)
                        {
                            case ScriptCommandType.Line:
                                currentCommand.SubCommands.Add(scriptCommand);
                                break;
                            case ScriptCommandType.Block:
                                currentCommand.SubCommands.Add(scriptCommand);
                                currentCommand = scriptCommand;
                                break;
                            case ScriptCommandType.End:
                                currentCommand = currentCommand.ParentCommand;
                                break;
                        }
                    }
                    catch
                    {
                        currentCommand = null;
                        GameManager.Instance.log.LogMessage("Script Command '" + lineToken[0] + "' Error At Line: " + (i + 1).ToString(), LogMessage.LogType.Error);
                        continue;
                    }
                    break;
                }
            }
            return currentCommand != null && currentCommand.SubCommands.Count > 0 ? currentCommand.SubCommands[0] : null;
        }
    }
}

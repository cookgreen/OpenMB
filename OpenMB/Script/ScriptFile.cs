using Mogre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using OpenMB.Script.Command;

namespace OpenMB.Script
{
	public class ScriptFile : IGameScript
    {
        private Stack<IScriptCommand> tempCommandStack;
		private Dictionary<string, Type> registeredCommand;
		private RootScriptCommand root;
		public ScriptContext Context { get; set; }
		public string FileName { get; set; }

        public string Name { get { return "Inner Script"; } }
        public string Extension { get { return ".script"; } }

        public List<IScriptCommand> Commands { get { return root.SubCommands; } }

        public ScriptFile()
		{
			Context = new ScriptContext(this);
			root = new RootScriptCommand();

			tempCommandStack = new Stack<IScriptCommand>();

			registeredCommand = ScriptCommandRegister.Instance.RegisteredCommand;
		}

		public ScriptFile(string scriptFileName)
		{
			Context = new ScriptContext(this);
			root = new RootScriptCommand();

			tempCommandStack = new Stack<IScriptCommand>();

			registeredCommand = ScriptCommandRegister.Instance.RegisteredCommand;

			FileName = scriptFileName;
			Parse(ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
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
            ScriptCommand previousCommand = root;
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
					EngineManager.Instance.log.LogMessage("Error Prase Script File At Line: '" + lineToken[0] + "' Error At Line: " + (i + 1).ToString(), LogMessage.LogType.Error);
					continue;
				}
				if (!registeredCommand.ContainsKey(lineToken[0]))
				{
					EngineManager.Instance.log.LogMessage("Script Command '" + lineToken[0] + "' Not Found At Line: " + (i + 1).ToString(), LogMessage.LogType.Warning);
					continue;
				}
				try
				{
					ScriptCommand currentCommand;
					currentCommand = Activator.CreateInstance(registeredCommand[lineToken[0]]) as ScriptCommand;
					currentCommand.ParentCommand = previousCommand;
					currentCommand.Context = Context;
					int tokenLength = lineToken.Length;
					
					for (int j = 1; j < tokenLength; j++)
					{
						currentCommand.PushArg(lineToken[j], j - 1);
					}
					switch (currentCommand.CommandType)
					{
						case ScriptCommandType.Line:
							previousCommand.AddSubCommand(currentCommand);
							break;
						case ScriptCommandType.ConditionControl:
						case ScriptCommandType.Block:
							previousCommand.AddSubCommand(currentCommand);
							previousCommand = currentCommand;
							break;
						case ScriptCommandType.End:
							switch (previousCommand.CommandName)
							{
								case "function":
									Context.RegisterFunction(previousCommand.CommandArgs[0], previousCommand.SubCommands);
									break;
							}
							previousCommand = previousCommand.ParentCommand;
							break;
						case ScriptCommandType.ConditionTurining:
							if (previousCommand.ParentCommand.CommandType == ScriptCommandType.ConditionTurining)
							{
								previousCommand = previousCommand.ParentCommand.ParentCommand;
							}
							else if(previousCommand.ParentCommand.CommandType == ScriptCommandType.ConditionControl)
							{
								previousCommand = previousCommand.ParentCommand;
							}
                            else
                            {
								//Error
								EngineManager.Instance.DisplayLogMessage("Condition turning statement must be inside the conditional control block", LogMessage.LogType.Error);
								continue;
                            }

							previousCommand = previousCommand.ParentCommand;

							previousCommand.AddSubCommand(currentCommand);
							previousCommand = currentCommand;

							if (previousCommand.CommandType == ScriptCommandType.ConditionTurining)
							{
								previousCommand = previousCommand.ParentCommand;
							}
							break;
					}
				}
				catch
				{
					previousCommand = null;
					EngineManager.Instance.log.LogMessage("Script Command '" + lineToken[0] + "' Error At Line: " + (i + 1).ToString(), LogMessage.LogType.Error);
					continue;
				}
			}
		}
		public IScriptCommand ParseOneLine(string groupName, int lineNo = 1)
		{
			if (lineNo < 1)
			{
				EngineManager.Instance.log.LogMessage("Invalid Line number!", LogMessage.LogType.Error);
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
						EngineManager.Instance.log.LogMessage("Error Prase Script File At Line: '" + lineToken[0] + "' Error At Line: " + (i + 1).ToString(), LogMessage.LogType.Error);
						continue;
					}
					if (!registeredCommand.ContainsKey(lineToken[0]))
					{
						EngineManager.Instance.log.LogMessage("Script Command '" + lineToken[0] + "' Not Found At Line: " + lineNo.ToString(), LogMessage.LogType.Warning);
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
							case ScriptCommandType.Conditional:
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
						EngineManager.Instance.log.LogMessage("Script Command '" + lineToken[0] + "' Error At Line: " + (i + 1).ToString(), LogMessage.LogType.Error);
						continue;
					}
					break;
				}
			}
			return currentCommand != null && currentCommand.SubCommands.Count > 0 ? currentCommand.SubCommands[0] : null;
		}
	}
}

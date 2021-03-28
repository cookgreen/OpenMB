﻿using OpenMB.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Script.Command
{
	public class ScriptCommand : IScriptCommand
	{
		public virtual string[] CommandArgs
		{
			get
			{
				return new string[] { "placeholderargs" };
			}
		}

		public virtual string CommandName
		{
			get
			{
				return "placeholder";
			}
		}

		public virtual ScriptCommandType CommandType
		{
			get
			{
				return ScriptCommandType.None;
			}
		}

		public virtual List<IScriptCommand> SubCommands
		{
			get;
			set;
		}

		public ScriptCommand ParentCommand
		{
			get;
			set;
		}

		public ScriptContext Context
		{
			get;
			set;
		}

		public virtual void Execute(params object[] executeArgs)
		{

		}

		public virtual void PushArg(string cmdArg, int index)
		{
			CommandArgs[index] = cmdArg;
		}


		protected bool isValidVariableName(string variableName)
		{
			return isLocalVariable(variableName) || isGlobalVariable(variableName);
		}

		protected bool isLocalVariable(string variableName)
        {
			return variableName.StartsWith("%");
		}

		protected bool isGlobalVariable(string variableName)
		{
			return variableName.StartsWith("$");
		}

		protected object getVariableValue(string variableName)
		{
			return
				variableName.StartsWith("%")
				? Context.GetLocalValue(variableName.Substring(1))
				: variableName.StartsWith("$")
					? ScriptGlobalVariableMap.Instance.GetVariable(variableName.Substring(1)).ToString()
					: variableName;
		}

		public void AddSubCommand(ScriptCommand scriptCommand)
		{
			SubCommands.Add(scriptCommand);
			scriptCommand.ParentCommand = this;
		}
	}
}

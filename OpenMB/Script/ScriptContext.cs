using OpenMB.Script.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Script
{
	public class ScriptContext
	{
		private ScriptLinkTable localTable;
		private Dictionary<string, string> localValMap;
		private Dictionary<string, ScriptFunction> functions;
		private Dictionary<string, ScriptTrigger> triggers;
		private Dictionary<string, string> returnValueTable;
		private ScriptFile file;
		public ScriptFile File
		{
			get
			{
				return file;
			}
		}

		public ScriptLinkTable LocalTable
		{
			get
			{
				return localTable;
			}
		}

		public ScriptContext(ScriptFile file)
		{
			this.file = file;
			localTable = new ScriptLinkTable();
			localValMap = new Dictionary<string, string>();
			functions = new Dictionary<string, ScriptFunction>();
			triggers = new Dictionary<string, ScriptTrigger>();
			returnValueTable = new Dictionary<string, string>();
		}

		public string GetLocalValue(string varname)
		{
			if (localValMap.ContainsKey(varname))
			{
				return localValMap[varname];
			}
			else
			{
				return null;
			}
		}

		public void ChangeLocalValue(string varname, string varvalue)
		{
			if (localValMap.ContainsKey(varname))
			{
				localValMap[varname] = varvalue;
			}
			else
			{
				localValMap.Add(varname, varvalue);
			}
		}

		public ScriptFunction GetFunction(string functionName)
		{
			if (functions.ContainsKey(functionName))
			{
				return functions[functionName];
			}
			else
			{
				return null;
			}
		}

		public ScriptTrigger GetTrigger(string triggerName)
		{
			if (functions.ContainsKey(triggerName))
			{
				return triggers[triggerName];
			}
			else
			{
				return null;
			}
		}

		public Dictionary<string, ScriptTrigger> GetTriggers()
		{
			return triggers;
		}

		public void RegisterFunction(string name, List<IScriptCommand> executeContent)
		{
			if (!functions.ContainsKey(name))
			{
				ScriptFunction func = new ScriptFunction();
				func.Name = name;
				func.Content = executeContent;
				functions.Add(name, func);
			}
		}

		public void RegisterTrigger(
			string name,
			string triggerCondition,
			float delayTime,
			float frozenTime,
			List<IScriptCommand> executeContent)
		{
			if (!functions.ContainsKey(name))
			{
				ScriptTrigger trigger = new ScriptTrigger();
				trigger.Name = name;
				trigger.delayTime = delayTime;
				trigger.frozenTime = frozenTime;
				trigger.Content = executeContent;
				triggers.Add(name, trigger);
			}
		}
	}
}

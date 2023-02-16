using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenMB.Game;
using Mogre;
using OpenMB.Script.Command;

namespace OpenMB.Script
{
	public class ScriptLoader : IGameScriptLoader
    {
		private ScriptFile currentFile = null;

		public string Extension { get { return ".script"; } }

		public ScriptContext currentContext
		{
			get
			{
				if (currentFile == null)
				{
					return null;
				}
				return currentFile.Context;
			}
		}
		public ScriptLoader()
		{
		}
		public IGameScript Parse(string scriptFileName, string groupName = null)
		{
			currentFile = new ScriptFile();
			currentFile.FileName = scriptFileName;
			if (!string.IsNullOrEmpty(groupName))
				groupName = ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME;
			currentFile.Parse(groupName);
			return currentFile;
		}

		public ScriptCommand ParseOneLine(string scriptFileName, out ScriptFile file, string groupName = null, int lineNo = 1)
		{
			currentFile = new ScriptFile();
			currentFile.FileName = scriptFileName;
			if (!string.IsNullOrEmpty(groupName))
				groupName = ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME;
			file = currentFile;
			return (ScriptCommand)currentFile.ParseOneLine(groupName, lineNo);
		}

		public void Execute(params object[] runArgs)
		{
			if (currentFile != null)
			{
				currentFile.Execute(runArgs);
			}
		}

		public void ExecuteFunction(ScriptFile scriptFile, string function, params object[] executeArgs)
		{
			var func = scriptFile.FindFunction(function);
			if (func != null)
			{
				func.Execute(executeArgs);
			}
		}
	}
}

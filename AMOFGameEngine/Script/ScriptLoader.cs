using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using AMOFGameEngine.Game;
using Mogre;
using AMOFGameEngine.Script.Command;

namespace AMOFGameEngine.Script
{
    public class ScriptLoader
    {
        private ScriptFile currentFile = null;

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
        public void Parse(string scriptFileName, string groupName = null)
        {
            currentFile = new ScriptFile();
            currentFile.FileName = scriptFileName;
            if (!string.IsNullOrEmpty(groupName))
                groupName = ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME;
            currentFile.Parse(groupName);
        }

        public void Execute(params object[] runArgs)
        {
            if (currentFile != null)
            {
                currentFile.Execute(runArgs);
            }
        }
    }
}

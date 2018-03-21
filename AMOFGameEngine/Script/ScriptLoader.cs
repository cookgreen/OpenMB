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
        public ScriptLoader()
        {
        }
        public void Parse(string scriptFileName, string groupName, params object[] runArgs)
        {
            ScriptFile file = new ScriptFile();
            file.FileName = scriptFileName;
            file.Parse(groupName, runArgs);
        }
    }
}

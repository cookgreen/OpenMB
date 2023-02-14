using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script.Lua
{
    public class LuaScript : IGameScript
    {
        public string Name { get { return "Lua"; } }

        public string Extension { get { return ".lua"; } }

        public void Execute(params object[] exeArgs)
        {
            //TODO:
        }

        public void Parse(string groupName, params object[] runArgs)
        {
            //TODO
        }
    }
}

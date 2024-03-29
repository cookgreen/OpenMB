﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script.Lua
{
    public class LuaScriptLoader : IGameScriptLoader
    {
        public string Extension { get { return ".py"; } }

        public IGameScript Parse(string scriptFileName, string groupName = null)
        {
            return new LuaScript();
        }
    }
}

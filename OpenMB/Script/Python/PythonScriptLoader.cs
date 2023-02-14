﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script.Python
{
    public class PythonScriptLoader : IGameScriptLoader
    {
        public IGameScript Parse(string scriptFileName, string groupName = null)
        {
            return new PythonScript();
        }
    }
}

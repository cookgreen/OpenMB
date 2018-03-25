using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Script
{
    public class ScriptContext
    {
        private Dictionary<string, string> localValMap;
        public ScriptContext()
        {
            localValMap = new Dictionary<string, string>();
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
            if(localValMap.ContainsKey(varname))
            {
                localValMap[varname] = varvalue;
            }
            else
            {
                localValMap.Add(varname, varvalue);
            }
        }
    }
}

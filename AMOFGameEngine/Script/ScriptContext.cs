using AMOFGameEngine.Script.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Script
{
    public class ScriptContext
    {
        private Dictionary<string, string> localValMap;
        private Dictionary<string, ScriptFunction> functions;
        public ScriptContext()
        {
            localValMap = new Dictionary<string, string>();
            functions = new Dictionary<string, ScriptFunction>();
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

        public ScriptFunction GetFunction(string functionName)
        {
            if(functions.ContainsKey(functionName))
            {
                return functions[functionName];
            }
            else
            {
                return null;
            }
        }

        public void RegisterFunction(string name, List<IScriptCommand> executeContent)
        {
            if(!functions.ContainsKey(name))
            {
                ScriptFunction func = new ScriptFunction();
                func.Name = name;
                func.Content = executeContent;
                functions.Add(name, func);
            }
        }
    }
}

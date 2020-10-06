using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script
{
    public class ScriptGlobalVariableMap
    {
        private static ScriptGlobalVariableMap instance;
		private Dictionary<string, object> globalVariableMap;
		private ScriptLinkTable globalVariableTable;
        public static ScriptGlobalVariableMap Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ScriptGlobalVariableMap();
                }
                return instance;
            }
        }

        public ScriptGlobalVariableMap()
        {
            globalVariableMap = new Dictionary<string, object>();

            globalVariableMap = new Dictionary<string, object>();
            globalVariableMap.Add("reg0", "0");
            globalVariableMap.Add("reg1", "0");
            globalVariableMap.Add("reg2", "0");
            globalVariableMap.Add("reg3", "0");
            globalVariableMap.Add("reg4", "0");
            globalVariableTable = ScriptValueRegister.Instance.GlobalValueTable;
        }

        public void AddVariable(string name, object value)
        {
            globalVariableMap[name] = value;
            ScriptValueStorage.Instance.Append(name, value);
        }

        public object GetVariable(string name)
        {
            return globalVariableMap[name];
        }
    }
}

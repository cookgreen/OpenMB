using AMOFGameEngine.Script.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Script
{
    class ScriptCommandRegister
    {
        public Dictionary<string, Type> RegisteredCommand { get; }
        private static ScriptCommandRegister instance;
        public static ScriptCommandRegister Instance
        {
            get
            {
                if (instance == null)
                    instance = new ScriptCommandRegister();
                return instance;
            }
        }

        public ScriptCommandRegister()
        {
            RegisteredCommand = new Dictionary<string, Type>();
            RegisteredCommand.Add("assign", typeof(AssignScriptCommand));
            RegisteredCommand.Add("call", typeof(CallScriptCommand));
            RegisteredCommand.Add("end", typeof(EndScriptCommand));
            RegisteredCommand.Add("function", typeof(FunctionScriptCommand));
            RegisteredCommand.Add("init_vector", typeof(InitVectorScriptCommand));
            RegisteredCommand.Add("loop", typeof(LoopScriptCommand));
            RegisteredCommand.Add("spawn", typeof(SpawnScriptCommand));
            RegisteredCommand.Add("store", typeof(StoreScriptCommand));
            RegisteredCommand.Add("team", typeof(TeamScriptCommand));
            RegisteredCommand.Add("vector_get_x", typeof(VectorGetXScriptCommand));
            RegisteredCommand.Add("vector_get_y", typeof(VectorGetYScriptCommand));
            RegisteredCommand.Add("vector_get_z", typeof(VectorGetZScriptCommand));
            RegisteredCommand.Add("vector_set_x", typeof(VectorSetXScriptCommand));
            RegisteredCommand.Add("vector_set_y", typeof(VectorSetYScriptCommand));
            RegisteredCommand.Add("vector_set_z", typeof(VectorSetZScriptCommand));
        }
    }
}

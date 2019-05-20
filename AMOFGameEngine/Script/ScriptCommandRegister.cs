using AMOFGameEngine.Script.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Script
{
    class ScriptCommandRegister
    {
        private Dictionary<string, Type> registerCommand;
        public Dictionary<string, Type> RegisteredCommand 
        {
            get
            {
                return registerCommand;
            }
        }
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
            
            registerCommand = new Dictionary<string, Type>();
            RegisteredCommand.Add("assign", typeof(AssignScriptCommand));
            RegisteredCommand.Add("agent_equip_item", typeof(AgentEquipItemScriptCommand));
            RegisteredCommand.Add("call", typeof(CallScriptCommand));
            RegisteredCommand.Add("change_hitpoint", typeof(ChangeHitpointScriptCommand));
            RegisteredCommand.Add("condition", typeof(TriggerConditionScriptCommand));
            RegisteredCommand.Add("end", typeof(EndScriptCommand));
            RegisteredCommand.Add("execute", typeof(TriggerExecuteSrciptCommand));
            RegisteredCommand.Add("function", typeof(FunctionScriptCommand));
            RegisteredCommand.Add("init_vector", typeof(InitVectorScriptCommand));
            RegisteredCommand.Add("loop", typeof(LoopScriptCommand));
            RegisteredCommand.Add("namespace", typeof(NamespaceScriptCommand));
            RegisteredCommand.Add("spawn", typeof(SpawnScriptCommand));
            RegisteredCommand.Add("store", typeof(StoreScriptCommand));
            RegisteredCommand.Add("team", typeof(TeamScriptCommand));
            RegisteredCommand.Add("trigger", typeof(TriggerScriptCommand));
            RegisteredCommand.Add("vector_get_x", typeof(VectorGetXScriptCommand));
            RegisteredCommand.Add("vector_get_y", typeof(VectorGetYScriptCommand));
            RegisteredCommand.Add("vector_get_z", typeof(VectorGetZScriptCommand));
            RegisteredCommand.Add("vector_set_x", typeof(VectorSetXScriptCommand));
            RegisteredCommand.Add("vector_set_y", typeof(VectorSetYScriptCommand));
            RegisteredCommand.Add("vector_set_z", typeof(VectorSetZScriptCommand));
        }
    }
}

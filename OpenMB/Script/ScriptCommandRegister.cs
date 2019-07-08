using OpenMB.Script.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Script
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
            RegisteredCommand.Add("change_map", typeof(ChangeMapScriptCommand));
            RegisteredCommand.Add("change_world_map", typeof(ChangeWorldMapScriptCommand));
            RegisteredCommand.Add("condition", typeof(TriggerConditionScriptCommand));
            RegisteredCommand.Add("end", typeof(EndScriptCommand));
            RegisteredCommand.Add("execute", typeof(TriggerExecuteSrciptCommand));
            RegisteredCommand.Add("function", typeof(FunctionScriptCommand));
            RegisteredCommand.Add("get_scene_prop_num", typeof(GetScenePropNumScriptCommand));
            RegisteredCommand.Add("get_scene_prop", typeof(GetScenePropScriptCommand));
            RegisteredCommand.Add("init_vector", typeof(InitVectorScriptCommand));
            RegisteredCommand.Add("init_list", typeof(InitListScriptCommand));
            RegisteredCommand.Add("list_get_num", typeof(ListGetValueNumScriptCommand));
            RegisteredCommand.Add("list_get_value", typeof(ListGetValueScriptCommand));
            RegisteredCommand.Add("list_remove_value", typeof(ListRemoveValueScriptCommand));
            RegisteredCommand.Add("list_set_value", typeof(ListSetValueScriptCommand));
            RegisteredCommand.Add("loop", typeof(LoopScriptCommand));
            RegisteredCommand.Add("move_scene_prop", typeof(MoveScenePropScriptCommand));
            RegisteredCommand.Add("namespace", typeof(NamespaceScriptCommand));
            RegisteredCommand.Add("remove_scene_prop", typeof(RemoveScenePropScriptCommand));
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

        public void RegisterNewCommand(string commandName, Type type)
        {
            if (!RegisteredCommand.ContainsKey(commandName))
            {
                if(!RegisteredCommand.ContainsValue(type))
                {
                    registerCommand.Add(commandName, type);
                }
                else
                {
                    GameManager.Instance.log.LogMessage(string.Format("The type with name `{0}` has been already registered into the engine!", type.FullName), LogMessage.LogType.Warning);
                }
            }
            else
            {
                GameManager.Instance.log.LogMessage(string.Format("The command with name `{0}` has been already registered into the engine!", commandName), LogMessage.LogType.Warning);
            }
        }
    }
}

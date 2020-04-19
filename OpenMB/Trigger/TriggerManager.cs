using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMB.Script;
using OpenMB.Script.Command;
using System.ComponentModel;
using OpenMB.Game;

namespace OpenMB.Trigger
{
    public class TriggerManager
    {
        public Dictionary<string, ScriptTrigger> Triggers { get; set; }
        private List<ScriptTrigger> triggerDelayQueue;
        private List<ScriptTrigger> triggerExecuteQueue;
        private List<ScriptTrigger> triggerForzenQueue;
        private static TriggerManager instance;

        private ScriptFile attachedScriptFile;
        private Dictionary<string, List<string>> hookedScriptFunctions;

        private GameWorld world;
        public static TriggerManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TriggerManager();
                }
                return instance;
            }
        }
        public TriggerManager()
        {
            Triggers = new Dictionary<string, ScriptTrigger>();
            triggerDelayQueue = new List<ScriptTrigger>();
            triggerExecuteQueue = new List<ScriptTrigger>();
            triggerForzenQueue = new List<ScriptTrigger>();

            attachedScriptFile = null;
            hookedScriptFunctions = new Dictionary<string, List<string>>();
        }

        public void Init(GameWorld world, ScriptContext context)
        {
            this.world = world;
            Triggers = context.GetTriggers();
        }
        public void Update(float timeSinceLastFrame)
        {
            for (int i = Triggers.Count - 1; i >= 0; i--)
            {
                if (Triggers.ElementAt(i).Value.delayTime > 0)
                {
                    Triggers.ElementAt(i).Value.CurrentDelay = Triggers.ElementAt(i).Value.delayTime;
                    triggerDelayQueue.Add(Triggers.ElementAt(i).Value);
                }
                else
                {
                    triggerExecuteQueue.Add(Triggers.ElementAt(i).Value);
                }
                Triggers.Remove(Triggers.ElementAt(i).Key);
            }

            for (int i = triggerDelayQueue.Count - 1; i >= 0; i--)
            {
                if (triggerDelayQueue[i].CurrentDelay > 0)
                {
                    triggerDelayQueue[i].CurrentDelay--;
                }
                else
                {
                    triggerExecuteQueue.Add(triggerDelayQueue[i]);
                    triggerDelayQueue.Remove(triggerDelayQueue[i]);
                }
            }

            for (int i = triggerExecuteQueue.Count - 1; i >= 0; i--)
            {
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += (o, e) =>
                {
                    int index = int.Parse(e.Argument.ToString());
                    triggerExecuteQueue[index].Execute(world);
                };
                worker.RunWorkerAsync(i);
            }

            for (int i = triggerForzenQueue.Count - 1; i >= 0; i--)
            {
                if (triggerForzenQueue[i].frozenTime == ScriptTrigger.TRIGGER_ONCE)
                {
                    continue;
                }

                if (triggerForzenQueue[i].frozenTime > 0)
                {
                    triggerForzenQueue[i].CurrentFrozen--;
                }
                else
                {
                    if(!Triggers.ContainsKey(triggerForzenQueue[i].Name))
                    {
                        Triggers.Add(triggerForzenQueue[i].Name, triggerForzenQueue[i]);
                    }
                    triggerForzenQueue.Remove(triggerForzenQueue[i]);
                }
            }
        }

        public void HookTriggerFunction(string triggerEvent, string functionName)
        {
            if (attachedScriptFile != null &&
                attachedScriptFile.Context.GetFunction(functionName) != null)
            {
                if (hookedScriptFunctions.ContainsKey(triggerEvent))
                {
                    if (!hookedScriptFunctions[triggerEvent].Contains(functionName))
                    {
                        hookedScriptFunctions[triggerEvent].Add(functionName);
                    }
                }
                else
                {
                    hookedScriptFunctions.Add(triggerEvent, new List<string>() { functionName });
                }
            }
        }

        public void TrigEvent(string triggerEvent, params object[] executeArgs)
        {
            if (attachedScriptFile != null)
            {
                if (!hookedScriptFunctions.ContainsKey(triggerEvent))
                {
                    hookedScriptFunctions.Add(triggerEvent, new List<string>());
                    return;
                }
                List<string> hookedFunctionNames = hookedScriptFunctions[triggerEvent];
                for (int i = 0; i < hookedFunctionNames.Count; i++)
                {
                    var func = attachedScriptFile.Context.GetFunction(hookedFunctionNames[i]);
                    if (func != null)
                    {
                        func.Execute(executeArgs);
                    }
                }
            }
        }

        private void Trigger_ExecuteCompleted(ScriptTrigger trigger)
        {
            triggerExecuteQueue.Remove(trigger);
            trigger.CurrentFrozen = trigger.frozenTime;
            triggerForzenQueue.Add(trigger);
        }

        public void ScenePropHit(GameObject gameObjInstance, GameObject gameObjInstance2)
        {
            var triggers = triggerExecuteQueue.Where(o => o.TriggerCondition == "ti_on_scene_prop_hit");
            foreach (var trigger in triggers)
            {
                trigger.Execute(world, gameObjInstance.ID, gameObjInstance2.ID);
            }
        }

		public void Exit()
		{
			triggerDelayQueue.Clear();
			triggerExecuteQueue.Clear();
			triggerForzenQueue.Clear();
		}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMOFGameEngine.Script;
using AMOFGameEngine.Script.Command;
using System.ComponentModel;
using AMOFGameEngine.Game;

namespace AMOFGameEngine.Trigger
{
    public class TriggerManager
    {
        public Dictionary<string, ScriptTrigger> Triggers { get; set; }
        private List<ScriptTrigger> triggerDelayQueue;
        private List<ScriptTrigger> triggerExecuteQueue;
        private List<ScriptTrigger> triggerForzenQueue;
        private static TriggerManager instance;
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

        private void Trigger_ExecuteCompleted(ScriptTrigger trigger)
        {
            triggerExecuteQueue.Remove(trigger);
            trigger.CurrentFrozen = trigger.frozenTime;
            triggerForzenQueue.Add(trigger);
        }
    }
}

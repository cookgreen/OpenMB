using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMOFGameEngine.Script;
using AMOFGameEngine.Script.Command;

namespace AMOFGameEngine.Trigger
{
    public class TriggerManager
    {
        public List<ITrigger> Triggers { get; set; }
        private static TriggerManager instance;
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
            Triggers = new List<ITrigger>();
        }
        public void Update(float timeSinceLastFrame)
        {
            for (int i = 0; i < Triggers.Count; i++)
            {
                if(Triggers[i].CheckCondition())
                    Triggers[i].Execute();
            }
        }

        public List<ITrigger> ParseTriggerFromFile(string fileName, string triggerName = null, params object[] paramters)
        {
            List<ITrigger> dummyTriggers = new List<ITrigger>();
            ScriptLoader loader = new ScriptLoader();
            var commands = loader.Parse(fileName);
            while (commands.Count > 0)
            {
                IScriptCommand command = commands.Dequeue();
                if (command is TriggerScriptCommand)
                {
                    DummyTrigger trigger = new DummyTrigger();
                    trigger.OnCheckTriggerCondition += (() => {
                        if (command.SubCommands.Count == 2)
                        {
                            var conditioncommandSegement = command.SubCommands[0];
                            for (int i = 0; i < conditioncommandSegement.SubCommands.Count; i++)
                            {
                                conditioncommandSegement.SubCommands[i].Execute(paramters);
                            }
                        }
                    });
                    trigger.OnExecuteTrigger += (() =>
                    {
                        if (command.SubCommands.Count == 2)
                        {
                            var executeCommandSegement = command.SubCommands[1];
                            for (int i = 0; i < executeCommandSegement.SubCommands.Count; i++)
                            {
                                executeCommandSegement.SubCommands[i].Execute(paramters);
                            }
                        }
                    });
                    Triggers.Add(trigger);
                    dummyTriggers.Add(trigger);
                }
            }
            return dummyTriggers;
        }
    }
}

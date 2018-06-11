using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}

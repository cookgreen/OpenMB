using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Trigger
{
    public class DummyTrigger : ITrigger
    {
        public event Action OnExecuteTrigger;
        public event Action OnCheckTriggerCondition;
        public int ExecuteTime
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int FreezeTime
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool CheckCondition()
        {
            if (OnCheckTriggerCondition != null)
            {
                OnCheckTriggerCondition();
            }
            return true;
        }

        public void Execute()
        {
            if (OnExecuteTrigger != null)
            {
                OnExecuteTrigger();
            }
        }
    }
}

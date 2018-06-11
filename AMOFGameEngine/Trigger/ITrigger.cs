using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Trigger
{
    public interface ITrigger
    {
        int ExecuteTime { get; }
        int FreezeTime { get; }
        bool CheckCondition();
        void Execute();
    }
}

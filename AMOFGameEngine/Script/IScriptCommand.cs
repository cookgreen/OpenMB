using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Script
{
    public interface IScriptCommand
    {
        string CommandName { get; }
        object[] CommandArgs { get; }
        void PushArg(string cmdArg, int index);
        void Execute(params object[] executeArgs);
    }
}

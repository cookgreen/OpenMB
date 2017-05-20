using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Mods
{
    public enum ModState
    {
        Stop,
        Run
    }
    public class ModEventArgs : EventArgs
    {
        public ModState modState;
        public string modName;
        public int modIndex;
    }
}

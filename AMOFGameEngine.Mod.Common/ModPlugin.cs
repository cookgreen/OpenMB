using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace AMOFGameEngine.Mods.Common
{
    class ModPlugin : Plugin
    {
        string pluginName;

        public ModPlugin()
        {
            mods = new List<Mod>();
        }

        public void AddMod(Mod mod)
        {
            mods.Add(mod);
        }

        public List<Mod> GetMods()
        {
            return mods;
        }

        public override void Initialise()
        {}

        public override void Install()
        {}

        public override string Name
        {
            get { return pluginName; }
        }

        public override void Shutdown()
        {}

        public override void Uninstall()
        {}

        protected List<Mod> mods;
    }
}

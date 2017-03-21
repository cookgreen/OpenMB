using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;


namespace AMOFGameEngine.Mod.Common
{
    public class ModPlugin : Plugin
    {
        private ModCollection mods;
        protected string name;

        protected ModCollection Mods
        {
            get { return mods; }
        }
        ModPlugin()
        {
            mods = new ModCollection();
        }

        public override void Initialise()
        {
            throw new NotImplementedException();
        }

        public override void Install()
        {
            throw new NotImplementedException();
        }

        public override string Name
        {
            get { return name; }
        }

        public override void Shutdown()
        {
            throw new NotImplementedException();
        }

        public override void Uninstall()
        {
            throw new NotImplementedException();
        }

        public void AddMod(Mod mod)
        {
            mods.Add(mod);
        }
    }
}

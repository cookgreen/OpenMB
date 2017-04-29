using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using AMOFGameEngine.Mods.Common;
using RGiesecke.DllExport;

namespace AMOFGameEngine.Mods.TestDemo
{
    public class Plugin : Mogre.Plugin
    {
        [DllExport("dllStartPlugin")]
        public static void dllStartPlugin()
        {
            if (singleton == null)
            {
                singleton = new Plugin();
            }
            Root.Singleton.InstallPlugin(singleton);
        }

        [DllExport("dllStopPlugin")]
        public static void dllStopPlugin()
        {
            Root.Singleton.UninstallPlugin(singleton);
            singleton = null;
        }

        protected static Plugin singleton;

        public override void Initialise()
        {
            TestModMain testMod = new TestModMain();
            testMod.Initialise();
        }

        public override void Install()
        {
            LogManager.Singleton.LogMessage("Mod has installed!");
        }

        public override string Name
        {
            get {
                return "TestMod";
            }
        }

        public override void Shutdown()
        {
        }

        public override void Uninstall()
        {
        }
    }
}

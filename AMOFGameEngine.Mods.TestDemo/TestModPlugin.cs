using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using AMOFGameEngine.Mods.Common;
using RGiesecke.DllExport;

namespace AMOFGameEngine.Mods.TestDemo
{
    public class TestModPlugin : Plugin
    {
        static TestModPlugin singleton;

        [DllExport("dllStartPlugin")]
        public static void dllStartPlugin()
        {
            if (singleton == null)
            {
                singleton = new TestModPlugin();
            }
            Root.Singleton.InstallPlugin(singleton);
        }

        [DllExport("dllStopPlugin")]
        public static void dllStopPlugin()
        {
            Root.Singleton.UninstallPlugin(singleton);
            singleton = null;
        }

        public override void Initialise()
        {
            TestModMain testMod = new TestModMain();
            testMod.Initilize();
        }

        public override void Install()
        {
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

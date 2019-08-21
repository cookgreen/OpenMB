using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MOIS;
using OpenMB.Script;
using OpenMB.Script.Command;

namespace OpenMB.Screen
{
    public class ScriptedScreen : Screen
    {
        private object[] args;
        public override event Action OnScreenExit;
        public event Action OnScreenRun;
        public UIScriptFile uiScript;
        public override string Name
        {
            get
            {
                return "Script";
            }
        }

        public ScriptedScreen(UIScriptFile uiScript)
        {
            this.uiScript = uiScript;
        }

        public override void Init(params object[] args)
        {
            this.args = args;
            uiScript.ExecuteSetup(args);
        }

        public override void Run()
        {
            if(OnScreenRun!=null)
            {
                OnScreenRun();
            }
        }

        public override void Update(float timeSinceLastFrame)
        {
            uiScript.ExecuteUpdate(args, timeSinceLastFrame);
        }

        public override void Exit()
        {
            if (OnScreenExit != null)
            {
                OnScreenExit();
            }
        }
    }
}

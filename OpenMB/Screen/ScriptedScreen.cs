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
		private string scrnID;
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

        public override void Init(params object[] args)
        {
            this.args = args;
			scrnID = args[1].ToString();
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

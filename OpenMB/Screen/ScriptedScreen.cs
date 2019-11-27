using System;
using System.Collections.Generic;
using System.Linq;
using OpenMB.Game;
using OpenMB.Mods;
using OpenMB.Mods.XML;
using OpenMB.Script;
using OpenMB.Widgets;

namespace OpenMB.Screen
{
	public class ScriptedScreen : Screen
    {
        private object[] args;
		private GameWorld world;
		private string uiLayoutID;
		private ModData modData;

		public override event Action OnScreenExit;
        public UIScriptFile uiScript;
        public override string Name
        {
            get
            {
                return "ScriptedScreen";
            }
        }

        public override void Init(params object[] args)
        {
            this.args = args;
			world = args[0] as GameWorld;
			uiLayoutID = args[1].ToString();
			modData = world.ModData;
		}

        public override void Run()
        {
			var uiLayoutData = modData.UILayoutInfos.Where(o => o.ID == uiLayoutID).FirstOrDefault();
			if (uiLayoutData == null)
			{
				throw new Exception("Invalid UILayout ID!");
			}

			foreach(var widgetData in uiLayoutData.Widgets)
			{
				createWidget(widgetData);
			}
		}

		private void createWidget(ModUILayoutWidgetDfnXml widgetData)
		{
		}

		public override void Update(float timeSinceLastFrame)
        {
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

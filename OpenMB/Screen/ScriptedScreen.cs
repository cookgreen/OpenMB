using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Mogre_Procedural.MogreBites;
using MOIS;
using OpenMB.Game;
using OpenMB.Mods;
using OpenMB.Mods.XML;
using OpenMB.Script;
using OpenMB.Script.Command;

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
			var func = SdkTrayManager.FactoryMethods[widgetData.Type];
			var type = GameManager.Instance.trayMgr.GetType();
			var method = type.GetMethod(func.MethodName);

			var widgetDataTypeProperties = widgetData.GetType().GetProperties();
			var sortedProperties = new PropertyInfo[widgetDataTypeProperties.Length - 1];

			for (int i = 0; i < sortedProperties.Length; i++)
			{
				if (i == 0)
				{
					var property = widgetDataTypeProperties.Where(o => o.Name == "TrayLocation").FirstOrDefault();
					sortedProperties[i] = property;
				}
				else if (i == 1)
				{
					var property = widgetDataTypeProperties.Where(o => o.Name == "Name").FirstOrDefault();
					sortedProperties[i] = property;
				}
				else if (i == 2)
				{
					var property = widgetDataTypeProperties.Where(o => o.Name == "Text").FirstOrDefault();
					sortedProperties[i] = property;
				}
				else if (i == 3)
				{
					var property = widgetDataTypeProperties.Where(o => o.Name == "Width").FirstOrDefault();
					sortedProperties[i] = property;
				}
				else
				{
					sortedProperties[i] = widgetDataTypeProperties[i + 1];
				}
			}

			var paramters = new object[func.NecessaryParamtersNumber];
			for (int i = 0; i < func.NecessaryParamtersNumber; i++)
			{
				paramters[i] = sortedProperties[i].GetValue(widgetData);
			}
			method.Invoke(GameManager.Instance.trayMgr, paramters);
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

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
        private ScriptFile scriptFile;
        private ScriptLoader loader;
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
            loader = new ScriptLoader();
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

            if (!string.IsNullOrEmpty(uiLayoutData.Script))
            {
                scriptFile = new ScriptFile(uiLayoutData.Script);
                ScriptLoader loader = new ScriptLoader();
                loader.ExecuteFunction(scriptFile, "uiInit", world, this);
            }
        }

		private void createWidget(ModUILayoutWidgetDfnXml widgetData)
		{
			Widget widget = UIManager.Instance.CreateWidget(modData, widgetData);
            if (widgetData.Type == "Button")
            {
                (widget as ButtonWidget).OnClick += ButtonWidget_OnClick;
            }
            widgets.Add(widget);
		}

        private void ButtonWidget_OnClick(object obj)
        {
            if (scriptFile != null)
            {
                loader.ExecuteFunction(
                    scriptFile, 
                    "uiMouseEventChanged", 
                    (obj as Widget).Name, 
                    world, 
                    this
                );
            }
        }

        public override void Update(float timeSinceLastFrame)
        {
        }

        public override void Exit()
        {
            UIManager.Instance.DestroyAllWidgets();
        }
    }
}

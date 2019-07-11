using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Mogre;
using OpenMB.Configure;
using OpenMB.Forms.Model;
using OpenMB.Localization;
using OpenMB.Core;

namespace OpenMB.Forms.Controller
{
    public class frmConfigureController
    {
        private Root r;
        private LOCATE selectedlocate;
        private GameConfigXml gameXmlConfig;
        public frmConfigure form;
        public AudioConfigure AudioConfig;
        public GameConfigure GameConfig;
        public GraphicConfigure GraphicConfig;
        public LOCATE CurrentLoacte
        {
            get
            {
                return selectedlocate;
            }
        }

        public frmConfigureController(frmConfigure form)
        {
            this.form = form;

            AudioConfig = new AudioConfigure();
            GameConfig = new GameConfigure();
            GraphicConfig = new GraphicConfigure();
            gameXmlConfig = GameConfigXml.Load("game.xml");

            r = new Root();

            form.Controller = this;
        }

        public void Init()
        {
            LoadGraphicConfigure();
            LoadAudioConfigure();
            LoadGameConfigure();

        }

        private void LoadGameConfigure()
        {
            selectedlocate = LocateSystem.Singleton.CovertReadableStringToLocate(gameXmlConfig.LocateConfig.CurrentLocate);
            GameConfig.CurrentSelectedLocate = LocateSystem.Singleton.ConvertLocateShortStringToReadableString(selectedlocate.ToString());
            GameConfig.IsEnableEditMode = gameXmlConfig.CoreConfig.IsEnableEditMode;
        }

        private void LoadGraphicConfigure()
        {
            if (!string.IsNullOrEmpty(gameXmlConfig.GraphicConfig.CurrentRenderSystem) && gameXmlConfig.GraphicConfig.Renderers.Count>0)
            {
                for (int i = 0; i < gameXmlConfig.GraphicConfig.Renderers.Count; i++)
                {
                    GraphicConfig.RenderSystemNames.Add(gameXmlConfig.GraphicConfig.Renderers[i].Name);
                }
                GraphicConfig.RenderSystem = gameXmlConfig.GraphicConfig.CurrentRenderSystem;
                GetGraphicSettingsByName(GraphicConfig.RenderSystem);
            }
            else
            {
                var renderers = r.GetAvailableRenderers();
                foreach(var renderer in renderers)
                {
                    GraphicConfig.RenderSystemNames.Add(renderer.Name);
                }
                if (GraphicConfig.RenderSystemNames.Count > 0)
                {
                    GetGraphicSettingsByName(GraphicConfig.RenderSystemNames[0]);
                }
            }
        }

        internal void InitLocates()
        {
            GameConfig.AvaliableLocates.Clear();
            foreach (var locateStr in LocateSystem.Singleton.AvaliableLocates)
            {
                GameConfig.AvaliableLocates.Add(LocateSystem.Singleton.ConvertLocateShortStringToReadableString(locateStr));
            }
        }

        private void LoadAudioConfigure()
        {
            AudioConfig.IsEnableSound = gameXmlConfig.AudioConfig.EnableSound;
            AudioConfig.IsEnableMusic = gameXmlConfig.AudioConfig.EnableMusic;
        }

        public void GetGraphicSettingsByName(string renderSystemName)
        {
            GraphicConfig.RenderParams.Clear();
            ConfigOptionMap configOptionMap = r.GetRenderSystemByName(renderSystemName).GetConfigOptions();
            if (gameXmlConfig.GraphicConfig.Renderers.Count > 0)
            {
                List<GameGraphicParameterConfigXml> dic = gameXmlConfig.GraphicConfig[gameXmlConfig.GraphicConfig.CurrentRenderSystem];
                List<string> graphicSettings = new List<string>();
                if (dic != null)
                {
                    for (int i = 0; i < configOptionMap.Count; i++)
                    {
                        GraphicConfig.RenderParams.Add(dic[i].Name + ":" + (configOptionMap[dic[i].Name].possibleValues.Contains(dic[i].Value) ? dic[i].Value : configOptionMap[dic[i].Name].possibleValues[0]));
                    }
                }
            }
            else
            {
                for (int i = 0; i < configOptionMap.Count; i++)
                {
                    GraphicConfig.RenderParams.Add(configOptionMap.ElementAt(i).Key + ":" + configOptionMap[configOptionMap.ElementAt(i).Key].possibleValues[0]);
                }
            }
        }

        public void InsertPossibleValue(string renderSystemName, string renderConfigKey, string renderConfigValue)
        {
            GraphicConfig.PossibleValues.Clear();
            ConfigOptionMap configOptionMap = r.GetRenderSystemByName(renderSystemName).GetConfigOptions();

            foreach (string psv in configOptionMap[renderConfigKey].possibleValues)
            {
                GraphicConfig.PossibleValues.Add(psv);
            }
            GraphicConfig.CurrentPossibleValue = renderConfigValue;
        }

        public void UpdateGraphicConfigByValue(string renderSystemName, string renderConfigKey, string renderConfigNewValue)
        {
            gameXmlConfig.GraphicConfig[gameXmlConfig.GraphicConfig.CurrentRenderSystem].Where(o => o.Name == renderConfigKey).First().Value = renderConfigNewValue;
            GetGraphicSettingsByName(renderSystemName);
        }

        public GameConfigXml SaveConfigure()
        {
            if (string.IsNullOrEmpty(gameXmlConfig.ModConfig.ModDir))
            {
                gameXmlConfig.ModConfig.ModDir = "Mods/";
            }
            gameXmlConfig.CoreConfig.IsEnableCheatMode = GameConfig.IsEnableCheatMode;
            gameXmlConfig.CoreConfig.IsEnableEditMode = GameConfig.IsEnableEditMode;
            gameXmlConfig.AudioConfig.EnableMusic = AudioConfig.IsEnableMusic;
            gameXmlConfig.AudioConfig.EnableSound = AudioConfig.IsEnableSound;
            gameXmlConfig.LocateConfig.CurrentLocate = GameConfig.CurrentSelectedLocate.ToString();
            gameXmlConfig.GraphicConfig.CurrentRenderSystem = GraphicConfig.RenderSystem;
            gameXmlConfig.GraphicConfig.Renderers.Clear();
            var renderers = r.GetAvailableRenderers();
            foreach (var renderer in renderers)
            {
                GameGraphicSectionConfigXml rendererConfig = new GameGraphicSectionConfigXml();
                rendererConfig.Name = renderer.Name;
                foreach (var configOption in r.GetRenderSystemByName(renderer.Name).GetConfigOptions())
                {
                    GameGraphicParameterConfigXml parameter = new GameGraphicParameterConfigXml();
                    parameter.Name = configOption.Key;
                    var findedInCurrentValues = GraphicConfig.RenderParams.Where(o => o.StartsWith(parameter.Name));
                    if (findedInCurrentValues.Count() > 0)
                    {
                        parameter.Value = findedInCurrentValues.First().Split(':')[1];
                    }
                    else
                    {
                        parameter.Value = configOption.Value.currentValue;
                    }
                    rendererConfig.Parameters.Add(parameter);
                }
                gameXmlConfig.GraphicConfig.Renderers.Add(rendererConfig);
            }
            gameXmlConfig.Save("game.xml");

            return gameXmlConfig;
        }
    }
}

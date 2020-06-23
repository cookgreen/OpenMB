using Mogre;
using MOIS;
using OpenMB.Configure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenMB.Core
{
    [XmlRoot("GameConfig")]
    public class GameConfigXml
    {
        [XmlElement("Graphic")]
        public GameGraphicConfigXml GraphicConfig { get; set; }
        [XmlElement("Audio")]
        public GameAudioConfigXml AudioConfig { get; set; }
        [XmlElement("Localized")]
        public GameLocateConfigXml LocateConfig { get; set; }
        [XmlElement("Mod")]
        public GameModConfigXml ModConfig { get; set; }
        [XmlElement("Network")]
        public GameNetworkConfigXml NetworkConfig { get; set; }
        [XmlElement("Game")]
        public GameCoreConfigXml CoreConfig { get; set; }
		[XmlElement("Input")]
		public GameInputConfigXml InputConfig { get; set; }
		[XmlElement("Resources")]
        public GameResourcesConfigXml ResourcesConfig { get; set; }
        [XmlElement("Plugins")]
        public GamePluginsConfigXml PluginConfig { get; set; }

        public GameConfigXml()
        {
            GraphicConfig = new GameGraphicConfigXml();
            AudioConfig = new GameAudioConfigXml();
            LocateConfig = new GameLocateConfigXml();
            ModConfig = new GameModConfigXml();
            NetworkConfig = new GameNetworkConfigXml();
            CoreConfig = new GameCoreConfigXml();
			InputConfig = new GameInputConfigXml();
            ResourcesConfig = new GameResourcesConfigXml();
            PluginConfig = new GamePluginsConfigXml();
		}

        public void GenerateDefaultConfig(params object[] param)
        {
            GraphicConfig.GenerateDefaultConfig(param);
            AudioConfig.GenerateDefaultConfig();
            LocateConfig.GenerateDefaultConfig();
            ModConfig.GenerateDefaultConfig();
            NetworkConfig.GenerateDefaultConfig();
            CoreConfig.GenerateDefaultConfig();
            InputConfig.GenerateDefaultConfig();
            ResourcesConfig.GenerateDefaultConfig();
            PluginConfig.GenerateDefaultConfig();
        }

        public static GameConfigXml Load(string configXml, params object[] param)
        {
            GameConfigXml config = null;
            if (File.Exists(configXml))
            {
                Stream stream = null;
                try
                {
                    stream = new FileStream(configXml, FileMode.Open, FileAccess.Read);
                    XmlSerializer serializer = new XmlSerializer(typeof(GameConfigXml));
                    config = serializer.Deserialize(stream) as GameConfigXml;
                }
                catch
                {

                }
                finally
                {
                    if (stream != null)
                    {
                        stream.Close();
                    }
                }
            }
            else
            {
                config = new GameConfigXml();
                config.GenerateDefaultConfig(param);
            }
            return config;
        }

        public bool Save(string configXml)
        {
            try
            {
                if (File.Exists(configXml))
                {
                    File.Delete(configXml);
                }
                XmlSerializer serializer = new XmlSerializer(typeof(GameConfigXml));
                FileStream stream = new FileStream(configXml, FileMode.OpenOrCreate, FileAccess.Write);
                serializer.Serialize(stream, this);
                stream.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    [XmlRoot("Game")]
    public class GameCoreConfigXml
    {
        [XmlElement]
        public bool IsEnableEditMode { get; set; }
        [XmlElement]
        public bool IsEnableCheatMode { get; set; }

        public void GenerateDefaultConfig()
        {
            IsEnableEditMode = false;
            IsEnableCheatMode = false;
        }
    }

	[XmlRoot("Input")]
	public class GameInputConfigXml
	{
		[XmlElement("Mapper")]
		public List<GameInputMapperConfigXml> Mappers { get; set; }

		public GameInputConfigXml()
		{
			Mappers = new List<GameInputMapperConfigXml>();
		}

        public void GenerateDefaultConfig()
        {
            
        }
    }

	[XmlRoot("Mapper")]
	public class GameInputMapperConfigXml
	{
		[XmlAttribute]
		public GameKeyCode GameKeyCode { get; set; }
		[XmlAttribute]
		public bool Combined { get; set; }
		[XmlElement("Key")]
		public List<GameInputMapperKeyConfigXml> Keys { get; set; }

		public KeyCollection GetKeyCollections()
		{
			KeyCollection kc = new KeyCollection();
			foreach(var key in Keys)
			{
				kc.keyCodes.Add(key.KeyCode);
			}
			return kc;
		}
	}

	[XmlRoot("Key")]
	public class GameInputMapperKeyConfigXml
	{
		[XmlText]
		public KeyCode KeyCode { get; set; }
	}

	[XmlRoot("Graphic")]
    public class GameGraphicConfigXml
    {
        [XmlElement("RenderSystem")]
        public string CurrentRenderSystem { get; set; }
        [XmlArray("Renderers")]
        [XmlArrayItem("Renderer")]
        public List<GameGraphicSectionConfigXml> Renderers { get; set; }
        public List<GameGraphicParameterConfigXml> this[string name]
        {
            get
            {
                return Renderers.Where(o => o.Name == name).Count() > 0 ? Renderers.Where(o => o.Name == name).First().Parameters : null;
            }
        }

        public GameGraphicConfigXml()
        {
            Renderers = new List<GameGraphicSectionConfigXml>();
        }

        public void GenerateDefaultConfig(params object[] param)
        {
            Root root = param[0] as Root;
            var renderers = root.GetAvailableRenderers();
            foreach (var render in renderers)
            {
                GameGraphicSectionConfigXml renderConfig = new GameGraphicSectionConfigXml();
                renderConfig.Name = render.Name;
                var renderParams = render.GetConfigOptions();
                foreach (var renderParam in renderParams)
                {
                    GameGraphicParameterConfigXml renderParamConfig = new GameGraphicParameterConfigXml();
                    renderParamConfig.Name = renderParam.Key;
                    renderParamConfig.Value = renderParam.Value.possibleValues[0];
                    renderConfig.Parameters.Add(renderParamConfig);
                }
                Renderers.Add(renderConfig);
            }
            CurrentRenderSystem = Renderers[0].Name;
        }
    }

    [XmlRoot("Renderer")]
    public class GameGraphicSectionConfigXml
    {
        [XmlElement]
        public string Name { get; set; }
        [XmlArray("Parameters")]
        [XmlArrayItem("Parameter")]
        public List<GameGraphicParameterConfigXml> Parameters { get; set; }

        public GameGraphicSectionConfigXml()
        {
            Parameters = new List<GameGraphicParameterConfigXml>();
        }
    }

    [XmlRoot("ConfigItem")]
    public class GameGraphicParameterConfigXml
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlText]
        public string Value { get; set; }
    }

    [XmlRoot("Audio")]
    public class GameAudioConfigXml
    {
        [XmlElement]
        public bool EnableSound { get; set; }
        [XmlElement]
        public bool EnableMusic { get; set; }

        public void GenerateDefaultConfig(params object[] param)
        {
            EnableSound = true;
            EnableMusic = true;
        }
    }

    [XmlRoot("Localized")]
    public class GameLocateConfigXml
    {
        [XmlElement]
        public string CurrentLocate { get; set; }

        internal void GenerateDefaultConfig(params object[] param)
        {
            CurrentLocate = "English";
        }
    }

    [XmlRoot("Mod")]
    public class GameModConfigXml
    {
        [XmlElement]
        public string ModDir { get; set; }

        public void GenerateDefaultConfig(params object[] param)
        {
            ModDir = "Mods";
        }
    }

    [XmlRoot("Network")]
    public class GameNetworkConfigXml
    {
        [XmlElement]
        public string Port { get; set; }

        public void GenerateDefaultConfig(params object[] param)
        {
            Port = "6539";
        }
    }

    [XmlRoot("Resources")]
    public class GameResourcesConfigXml
    {
        [XmlElement("ResourceRootDir")]
        public string ResourceRootDir { get; set; }
        [XmlElement("Resource")]
        public List<GameResourceConfigXml> Resources { get; set; }
        public GameResourcesConfigXml()
        {
            Resources = new List<GameResourceConfigXml>();
        }

        public void GenerateDefaultConfig(params object[] param)
        {
            ResourceRootDir = "./Media";
            Resources.Add(new GameResourceConfigXml()
            {
                Type = "FileSystem",
                ResourceLocs = new List<string>()
                {
                    "./Media/Ogre/materials/programs",
                    "./Media/Ogre/materials/scripts",
                    "./Media/Ogre/materials/textures",
                    "./Media/Ogre/materials/textures/nvidia",
                    "./Media/Ogre/materials/textures/SSAO",
                    "./Media/Ogre/models",
                }
            });
            Resources.Add(new GameResourceConfigXml()
            {
                Type = "Zip",
                ResourceLocs = new List<string>()
                {
                    "./Media/Ogre/packs/cubemapsJS.zip",
                    "./Media/Ogre/packs/OgreCore.zip",
                    "./Media/Ogre/packs/ogretestmap.zip",
                    "./Media/Ogre/packs/SdkTrays.zip",
                    "./Media/Ogre/packs/Sinbad.zip",
                    "./Media/Ogre/packs/skybox.zip",
                }
            });
        }
    }

    [XmlRoot("Resource")]
    public class GameResourceConfigXml
    {
        [XmlAttribute]
        public string Type { get; set; }
        [XmlElement("ResourceLoc")]
        public List<string> ResourceLocs;
    }

    [XmlRoot("Plugins")]
    public class GamePluginsConfigXml
    {
        [XmlElement]
        public string PluginRootDir { get; set; }
        [XmlElement("Plugin")]
        public List<string> Plugins { get; set; }

        public GamePluginsConfigXml()
        {
            Plugins = new List<string>();
        }

        public void GenerateDefaultConfig(params object[] param)
        {
            PluginRootDir = "./Plugins/";
            string pluginRootFullPath = Path.Combine(Environment.CurrentDirectory, PluginRootDir);
            DirectoryInfo di = new DirectoryInfo(pluginRootFullPath);
            foreach (var file in di.EnumerateFiles())
            {
                if (file.Extension == ".dll")
                {
                    Plugins.Add(file.Name);
                }
            }
        }
    }
}

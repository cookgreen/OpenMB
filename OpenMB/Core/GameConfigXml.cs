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

        public GameConfigXml()
        {
            GraphicConfig = new GameGraphicConfigXml();
            AudioConfig = new GameAudioConfigXml();
            LocateConfig = new GameLocateConfigXml();
            ModConfig = new GameModConfigXml();
            NetworkConfig = new GameNetworkConfigXml();
            CoreConfig = new GameCoreConfigXml();
        }

        public static GameConfigXml Load(string configXml)
        {
            GameConfigXml config = new GameConfigXml();
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
    }

    [XmlRoot("Localized")]
    public class GameLocateConfigXml
    {
        [XmlElement]
        public string CurrentLocate { get; set; }
    }

    [XmlRoot("Mod")]
    public class GameModConfigXml
    {
        [XmlElement]
        public string ModDir { get; set; }
    }

    [XmlRoot("Network")]
    public class GameNetworkConfigXml
    {
        [XmlElement]
        public string Port { get; set; }
    }
}

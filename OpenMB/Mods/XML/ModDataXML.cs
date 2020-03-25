using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenMB.Mods.XML
{
    public class ModDataXML
	{
		[XmlElement("Animations")]
		public string animationXml { get; set; }
		[XmlElement("Characters")]
        public string characterXML { get; set; }
		[XmlElement("Cursors")]
		public string cursorsXml { get; set; }
		[XmlElement("Music")]
        public string musicXML { get; set; }
        [XmlElement("Sound")]
        public string soundXML { get; set; }
        [XmlElement("Items")]
        public string itemXML { get; set; }
        [XmlElement("ItemTypes")]
        public string itemTypeXML { get; set; }
        [XmlElement("Sides")]
        public string sideXML { get; set; }
        [XmlElement("Skin")]
        public string skinXML { get; set; }
        [XmlElement("Maps")]
        public string mapsXml { get; set; }
        [XmlElement("WorldMaps")]
        public string worldMapsXML { get; set; }
        [XmlElement("Locations")]
        public string locationsXML { get; set; }
        [XmlElement("Skeletons")]
        public string skeletonsXML { get; set; }
        [XmlElement("SceneProps")]
        public string scenePropsXML { get; set; }
        [XmlElement("Models")]
        public string modelsXml { get; set; }
		[XmlElement("Menus")]
		public string menusXml { get; set; }
		[XmlElement("Strings")]
		public string stringsXml { get; set; }
		[XmlElement("UILayouts")]
		public string uiLayoutsXml { get; set; }
		[XmlElement("MapTemplates")]
		public string mapTemplatesXml { get; set; }
        [XmlElement("Vehicles")]
        public string vehicleXml { get; set; }
        [XmlElement]
        public ModDataDirXML DataDir { get; set; }
	}

    public class ModDataDirXML
    {
        [XmlElement]
        public string MapDir { get; set; }
        [XmlElement]
        public string MusicDir { get; set; }
        [XmlElement]
        public string ScriptDir { get; set; }
    }
}

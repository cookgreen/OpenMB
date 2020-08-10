using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenMB.UI.Skin
{
    /// <summary>
    /// Skin Describe Xml file
    /// </summary>
    [XmlRoot("Skin")]
    public class SkinFile
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlElement("SkinElement")]
        public List<SkinElement> Rects { get; set; }

        public static SkinFile Load(string skinFile)
		{
            XmlSerializer serializer = new XmlSerializer(typeof(SkinFile));
            SkinFile skin = serializer.Deserialize(new FileStream(skinFile, FileMode.Open, FileAccess.Read)) as SkinFile;
            return skin;
		}
    }

    [XmlRoot("SkinElement")]
    public class SkinElement
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlText]
        public string Value { get; set; }
        [XmlElement("SkinElement")]
        public List<SkinElement> Elements { get; set; }
    }
}

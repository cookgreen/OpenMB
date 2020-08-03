using System;
using System.Collections.Generic;
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
    public class SkinDefine
    {
        [XmlElement]
        public string Name { get; set; }
        [XmlElement("SkinRect")]
        public List<SkinDefineRect> Rects { get; set; }
    }

    [XmlRoot("SkinRect")]
    public class SkinDefineRect
    {
        [XmlElement("BoderElement")]
        public SkinDefineRectBorderElement BorderSkin { get; set; }
        [XmlElement("BackgroundElement")]
        public SkinDefineRectElement BackgroundSkin { get; set; }
    }

    [XmlRoot("BorderElement")]
    public class SkinDefineRectBorderElement
    {
        public string Picture { get; set; }
        public string BorderSize { get; set; }
        public string BorderTopleftUV { get; set; }
        public string BorderTopUV { get; set; }
        public string BorderToprightUV { get; set; }
        public string BorderLeftUV { get; set; }
        public string BorderRightUV { get; set; }
        public string BorderBottomleftUV { get; set; }
        public string BorderBottomUV { get; set; }
        public string BorderBottomrightUV { get; set; }
    }

    [XmlRoot("Element")]
    public class SkinDefineRectElement
    {
        public string Picture { get; set; }
        public string UVCoords { get; set; }
    }
}

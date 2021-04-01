using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenMB.Campaign.XML
{
    [XmlRoot("Campaign")]
    public class CampaignListXml
    {
        [XmlAttribute]
        public string id { get; set; }
        [XmlAttribute]
        public string desc { get; set; }
        [XmlAttribute]
        public string type { get; set; }
        [XmlAttribute]
        public string value { get; set; }
    }

    [XmlRoot("Campaigns")]
    public class CampaignListsXml
    {
        [XmlElement("Campaign")]
        public List<CampaignListXml> Campaigns { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using OpenMB.Mods.XML;
using System.Xml;

namespace OpenMB.Mods
{
    public class ModXmlLoader
    {
        protected string modPath;
        protected XmlDocument doc;

        public ModXmlLoader(string path)
        {
            modPath = path;
        }

        public virtual bool Load<T>(out T ModXMLData)
        {
            try
            {
                XmlSerializer xr = new XmlSerializer(typeof(T));
                ModXMLData = (T)xr.Deserialize(new FileStream(modPath, FileMode.Open, FileAccess.Read));
                return true;
            }
            catch(Exception ex)
            {
                GameManager.Instance.log.LogMessage(ex.ToString(), LogMessage.LogType.Error);
                ModXMLData = default(T);
                return false;
            }
        }

        public bool Save<T>(T xmlData)
        {
            try
            {
                XmlSerializer xr = new XmlSerializer(typeof(T));
                xr.Serialize(new FileStream(modPath, FileMode.OpenOrCreate, FileAccess.Write), xmlData);
                return true;
            }
            catch (Exception ex)
            {
                GameManager.Instance.log.LogMessage(ex.ToString(), LogMessage.LogType.Error);
                return false;
            }
        }
    }
}

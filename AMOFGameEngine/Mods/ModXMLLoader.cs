using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using AMOFGameEngine.Mods.XML;

namespace AMOFGameEngine.Mods
{
    public class ModXMLLoader
    {
        private string modPath;
        public ModXML ModXMLData;

        public ModXMLLoader(string path)
        {
            modPath = path;
        }

        public bool Load()
        {
            try
            {
                XmlSerializer xr = new XmlSerializer(typeof(ModXML));
                ModXMLData = xr.Deserialize(new FileStream(modPath, FileMode.Open, FileAccess.Read)) as ModXML;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

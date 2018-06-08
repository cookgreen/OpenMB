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

        public ModXMLLoader(string path)
        {
            modPath = path;
        }

        public bool Load<T>(out T ModXMLData)
        {
            try
            {
                XmlSerializer xr = new XmlSerializer(typeof(T));
                ModXMLData = (T)xr.Deserialize(new FileStream(modPath, FileMode.Open, FileAccess.Read));
                return true;
            }
            catch
            {
                ModXMLData = default(T);
                return false;
            }
        }
    }
}

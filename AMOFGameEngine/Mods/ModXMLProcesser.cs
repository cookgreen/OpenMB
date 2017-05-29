using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using AMOFGameEngine.Mods.Common;

namespace AMOFGameEngine.Mods
{
    public class ModXMLProcesser
    {
        string fileName;
        ModXML xmlFile;

        public ModXMLProcesser(string fileName)
        {
            this.fileName = fileName;
        }

        public ModXML ProcessModFile()
        {
            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Read))
                {
                    XmlSerializer xr = new XmlSerializer(typeof(ModXML));
                    xmlFile = (ModXML)xr.Deserialize(fs);
                    return xmlFile;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}

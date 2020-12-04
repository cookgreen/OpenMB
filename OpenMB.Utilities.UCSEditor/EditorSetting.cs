using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenMB.Utilities.UCSEditor
{
    [XmlRoot]
    public class EditorSetting
    {
        [XmlElement("GoogleTranslateAPI")]
        public GoogleTranslateAPISetting GoogleTranslateAPISetting { get; set; }

        public static EditorSetting Read()
        {
            FileStream stream = new FileStream("UCSEditorSetting.xml", FileMode.Open, FileAccess.Read);
            XmlSerializer serializer = new XmlSerializer(typeof(EditorSetting));
            EditorSetting editorSetting = serializer.Deserialize(stream) as EditorSetting;
            return editorSetting;
        }
    }

    [XmlRoot]
    public class GoogleTranslateAPISetting
    {
        [XmlArray("TranslateLanguages")]
        [XmlArrayItem("TranslateLanguage")]
        public List<GoogleTranslateAPITranslateLanguage> TranslateLanguages { get; set; }

        public string this[string displayName]
        {
            get
            {
                return TranslateLanguages.Where(o => o.DisplayName == displayName).FirstOrDefault().GoogleTransAPILocate;
            }
        }
    }

    [XmlRoot("GoogleTranslateAPITranslateLanguage")]
    public class GoogleTranslateAPITranslateLanguage
    {
        [XmlAttribute]
        public string Locate { get; set; }
        [XmlAttribute]
        public string DisplayName { get; set; }
        [XmlText]
        public string GoogleTransAPILocate { get; set; }
    }
}

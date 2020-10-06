using OpenMB.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Mods
{
    //parse the string like `text='(=localizedStringID) DefaultText will show up if localized String don't exist'`"
    public class ModLocalizedField
    {
        private string defaultText;
        private string localizedStringID;

        public ModLocalizedField(string localizedStringID, string defaultText)
        {
            this.localizedStringID = localizedStringID;
            this.defaultText = defaultText;
        }

        public string LocalizedStringID { get { return localizedStringID; } }
        public string DefaultText { get { return defaultText; } }
    }
    public class ModLocalizedFieldManager
    {
        private Dictionary<string, ModLocalizedField> localizedFields;

        private static ModLocalizedFieldManager instance;
        private ModData modData;

        public static ModLocalizedFieldManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ModLocalizedFieldManager();
                }
                return instance;
            }
        }

        public ModLocalizedFieldManager()
        {
            localizedFields = new Dictionary<string, ModLocalizedField>();
        }

        public void InitMod(ModData modData)
        {
            this.modData = modData;
        }

        public bool IsLocalizaedField(string xmlTextField)
        {
            return xmlTextField.StartsWith("(=") && xmlTextField.Contains(")");
        }

        public void Parse(string xmlTextField)
        {
            if (IsLocalizaedField(xmlTextField))
            {
                string[] tokens = xmlTextField.Split(')');
                string localizedStrID = tokens[0].Substring(2);
                string defaultText = tokens[1].Trim();
                ModLocalizedField modLocalizedField = new ModLocalizedField(localizedStrID, defaultText);
                localizedFields[localizedStrID] = modLocalizedField;
            }
        }

        public string GetLocalizedString(string ID)
        {
            if (localizedFields.ContainsKey(ID))
            {
                var field = localizedFields[ID];
                return LocateSystem.Instance.GetLocalizedString(ID, field.DefaultText, modData.Manifest.ID);
            }
            else
            {
                return null;
            }
        }
    }
}

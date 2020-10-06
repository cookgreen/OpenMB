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
    public class ModLocalizedFieldResolver
    {
        public static ModLocalizedField Parse(string xmlTextField)
        {
            if (xmlTextField.StartsWith("(=") && xmlTextField.Contains(")"))
            {
                string[] tokens = xmlTextField.Split(')');
                string localizedStrID = tokens[0].Substring(2);
                string defaultText = tokens[1].Trim();
                ModLocalizedField modLocalizedField = new ModLocalizedField(localizedStrID, defaultText);
                return modLocalizedField;
            }
            else
            {
                return null;
            }
        }
    }
}

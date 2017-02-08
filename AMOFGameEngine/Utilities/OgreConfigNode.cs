using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace AMOFGameEngine.Utilities
{
    public struct OgreConfigNode
    {
        private ConfigFile.SettingsMultiMap _settings;
        public ConfigFile.SettingsMultiMap settings
        {
            get
            {
                return _settings;
            }
            set
            {
                _settings = value;
            }
        }

        private string _section;
        public string section
        {
            get
            {
                return _section;
            }
            set
            {
                _section = value;
            }
        }
    }
}

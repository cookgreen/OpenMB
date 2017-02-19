using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Utilities
{
    class OgreConfigNode
    {
        private string section;

        public string Section
        {
            get { return section; }
            set { section = value; }
        }

        private Dictionary<string, string> settings;

        public Dictionary<string, string> Settings
        {
            get { return settings; }
            set { settings = value; }
        }
        public OgreConfigNode()
        {
            settings = new Dictionary<string, string>();
        }
    }
}

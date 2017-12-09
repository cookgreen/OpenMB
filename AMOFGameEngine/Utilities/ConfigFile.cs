using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Utilities
{
    public class ConfigFileKeyValuePair
    {
        private string key;
        private string val;
        public string Key
        {
            get
            {
                return key;
            }
            set
            {
                key = value;
            }
        }
        public string Value
        {
            get
            {
                return val;
            }
            set
            {
                val = value;
            }
        }
    }

    public class ConfigFileSection
    {
        private string name;
        private List<ConfigFileKeyValuePair> keyValuePairs;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        public List<ConfigFileKeyValuePair> KeyValuePairs
        {
            get
            {
                return keyValuePairs;
            }
            set
            {
                keyValuePairs = value;
            }
        }
        public ConfigFileSection()
        {
            keyValuePairs = new List<ConfigFileKeyValuePair>();
        }
    }

    public class ConfigFile
    {
        private string name;
        private List<ConfigFileSection> sections;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        public List<ConfigFileSection> Sections
        {
            get
            {
                return sections;
            }
            set
            {
                sections = value;
            }
        }
        public ConfigFile()
        {
            sections = new List<ConfigFileSection>();
        }
    }
}

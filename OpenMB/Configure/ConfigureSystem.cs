using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Configure
{
    public enum ConfigureType
	{
		INI,
        XML
    }
    /// <summary>
    /// Main class for loading/saveing config file
    /// </summary>
    public class ConfigureSystem
    {
        private IConfigParser parser;
        private static ConfigureSystem instance;
        public static ConfigureSystem Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ConfigureSystem();
                }
                return instance;
            }
        }

        public IConfigParser GetParser(ConfigureType configureType)
        {
            if (parser != null)
            {
                parser = null;
            }
            switch(configureType)
            {
                case ConfigureType.INI:
                    parser = new IniConfigFileParser();
                    break;
            }
            return parser;
        }

        public IniConfigFileParser GetIniParser()
        {
            return (IniConfigFileParser)GetParser(ConfigureType.INI);
        }
    }
}

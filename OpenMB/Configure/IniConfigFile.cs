using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Configure
{
    public class IniConfigFileKeyValuePair
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

    public class IniConfigFileSection
    {
        private string name;
        private List<IniConfigFileKeyValuePair> keyValuePairs;
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
        public List<IniConfigFileKeyValuePair> KeyValuePairs
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
        public IniConfigFileSection()
        {
            keyValuePairs = new List<IniConfigFileKeyValuePair>();
        }
        public string this[string key]
        {
            get
            {
                string resultValue;
                var resultKeyValuePair = from kpl in keyValuePairs
                                         where kpl.Key == key
                                         select kpl;
                if (resultKeyValuePair.Count() > 0)
                {
                    resultValue = resultKeyValuePair.ElementAt(0).Value;
                }
                else
                {
                    resultValue = null;
                }
                return resultValue;
            }
            set
            {
                var resultKeyValuePair = from kpl in keyValuePairs
                                         where kpl.Key == key
                                         select kpl;
                if (resultKeyValuePair.Count() > 0)
                {
                    resultKeyValuePair.First().Value = value;
                }
            }
        }
        public string GetValueByKey(string key)
        {
            string resultValue;
            var resultKeyValuePair = from kpl in keyValuePairs
                                     where kpl.Key == key
                                     select kpl;
            if (resultKeyValuePair.Count() > 0)
            {
                resultValue = resultKeyValuePair.ElementAt(0).Value;
            }
            else
            {
                resultValue = null;
            }
            return resultValue;
        }
    }

    public class IniConfigFile : IConfigFile
    {
        private string name;
        private string path;
        private List<IniConfigFileSection> sections;
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
        public string Path
        {
            get
            {
                return path;
            }
            set
            {
                path = value;
            }
        }
        public List<IniConfigFileSection> Sections
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
        public IniConfigFile()
        {
            sections = new List<IniConfigFileSection>();
        }

        public IniConfigFileSection this[string sectionName]
        {
            get
            {
                IniConfigFileSection resultSection;

                var resultSections = from section in sections
                                     where section.Name == sectionName
                                     select section;
                if (resultSections.Count() > 0)
                {
                    resultSection = resultSections.ElementAt(0);
                }
                else
                {
                    resultSection = null;
                }

                return resultSection;
            }
        }

        public IniConfigFileSection GetSectionByName(string sectionName)
        {
            IniConfigFileSection resultSection;

            var resultSections = from section in sections
                                 where section.Name == sectionName
                                 select section;
            if (resultSections.Count() > 0)
            {
                resultSection = resultSections.ElementAt(0);
            }
            else
            {
                resultSection = null;
            }

            return resultSection;
        }
    }
}

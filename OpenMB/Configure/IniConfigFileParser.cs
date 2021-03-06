﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace OpenMB.Configure
{
    public class IniConfigFileParser : IConfigParser
    {
        public IConfigFile Load(string filePath)
        {
            IniConfigFile conf = new IniConfigFile();
            conf.Name = filePath;
            IniConfigFileSection currentSection = null;
            int counter = 0;
            using (StreamReader sr = new StreamReader(filePath))
            {
                while (sr.Peek() != -1)
                {
                    string line = sr.ReadLine();
                    if (line.StartsWith("#"))//Skip comments
                    {
                        continue;
                    }
                    else if (line.StartsWith("[") && line.EndsWith("]"))
                    {
                        currentSection = new IniConfigFileSection();
                        currentSection.Name = line.Substring(1, line.IndexOf(']') - 1);
                        conf.Sections.Add(currentSection);
                    }
                    else if (counter == 0 && line.Split('=').Length == 2)//No section
                    {
                        currentSection = new IniConfigFileSection();
                        currentSection.Name = string.Empty;
                        currentSection.KeyValuePairs.Add(new IniConfigFileKeyValuePair()
                            {
                                Key = line.Split('=')[0],
                                Value = line.Split('=')[1]
                            });
                        conf.Sections.Add(currentSection);
                    }
                    else if (line.Split('=').Length == 2)
                    {
                        currentSection.KeyValuePairs.Add(new IniConfigFileKeyValuePair()
                        {
                            Key = line.Split('=')[0],
                            Value = line.Split('=')[1]
                        });
                    }
                    counter++;
                }
            }

            return conf;
        }

        public bool Save(IConfigFile cf)
        {
            try
            {
                IniConfigFile conf = (IniConfigFile)cf;
                using (StreamWriter sw = new StreamWriter(conf.Name))
                {
                    for (int i = 0; i < conf.Sections.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(conf.Sections[i].Name))
                        {
                            string sectionLine = string.Format("[{0}]", conf.Sections[i].Name);
                            sw.WriteLine(sectionLine);
                        }
                        for (int j = 0; j < conf.Sections[i].KeyValuePairs.Count; j++)
                        {
                            string keyValueLine = string.Format("{0}={1}", conf.Sections[i].KeyValuePairs[j].Key, conf.Sections[i].KeyValuePairs[j].Value);
                            sw.WriteLine(keyValueLine);
                        }
                        sw.WriteLine();
                    }
                    sw.Flush();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

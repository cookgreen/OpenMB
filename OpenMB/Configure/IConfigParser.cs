using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Configure
{
    public interface IConfigParser
    {
        IConfigFile Load(string configFilePath);

        bool Save(IConfigFile configFile);
    }
}

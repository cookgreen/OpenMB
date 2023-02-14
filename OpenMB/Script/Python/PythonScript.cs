using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script.Python
{
    public class PythonScript : IGameScript
    {
        public string Name { get { return "Python"; } }

        public string Extension { get { return ".py"; } }

        public void Execute(params object[] exeArgs)
        {
        }

        public void Parse(string groupName, params object[] runArgs)
        {
        }
    }
}

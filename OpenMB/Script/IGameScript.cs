using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script
{
    public interface IGameScript
    {
        string Name { get; }
        string Extension { get; }
        void Parse(string groupName, params object[] runArgs);
        void Execute(params object[] exeArgs);
    }
}

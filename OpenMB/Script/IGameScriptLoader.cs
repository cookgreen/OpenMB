using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script
{
    public interface IGameScriptLoader
    {
        string Extension { get; }
        IGameScript Parse(string scriptFileName, string groupName = null);
    }
}

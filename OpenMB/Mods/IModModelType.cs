using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Mods
{
    /// <summary>
    /// Define a mod model type which will be used in processing item and scene props
    /// </summary>
    public interface IModModelType
    {
        /// <summary>
        /// Type Name
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Process logic
        /// </summary>
        /// <param name="mod"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        object Process(ModData mod, params object[] param);
    }
}

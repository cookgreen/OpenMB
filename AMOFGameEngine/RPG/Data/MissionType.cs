using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.RPG.Data
{
    /// <summary>
    /// Mission State
    /// </summary>
    public enum MissonState
    {
        /// <summary>
        /// This Mission is processing
        /// </summary>
        Processing,
        /// <summary>
        /// This Mission is finished
        /// </summary>
        Finished,
        /// <summary>
        /// This Mission has failed
        /// </summary>
        Failed,
        /// <summary>
        /// This Mission has successed
        /// </summary>
        Successed
    }
}

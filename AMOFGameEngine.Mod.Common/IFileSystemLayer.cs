using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mogre_Procedural.MogreBites
{
    interface IFileSystemLayer
    {
        /// <summary>
        /// Search for the given config file in the user's home path. If it can't
		/// be found there, the function falls back to the system-wide install
		///	path for Ogre config files.
        /// </summary>
        /// <param name="fileName">The config file name (without path)</param>
        /// <returns>The full path to the config file.</returns>
        string getConfigFilePath(string fileName);

        /// <summary>
        /// Find a path where the given filename can be written to. This path 
		///	will usually be in the user's home directory. This function should
		///	be used for any output like logs and graphics settings. 
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>The full path to a writable location for the given filename.</returns>
        string getWritablePath(string fileName);
    }
}

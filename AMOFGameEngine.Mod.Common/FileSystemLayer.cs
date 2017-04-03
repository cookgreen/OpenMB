using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Mogre;

namespace Mogre_Procedural.MogreBites
{
    public class FileSystemLayer : IFileSystemLayer
    {
        StringVector mConfigPaths;
        string mHomePath;

        public FileSystemLayer(string subDir)
        {
            getConfigPaths();
            prepareUserHome(subDir);
        }
        public string getConfigFilePath(string fileName)
        {
            throw new NotImplementedException();
        }

        public string getWritablePath(string fileName)
        {
            throw new NotImplementedException();
        }

        void getConfigPaths()
        {

        }

        void prepareUserHome(string subDir)
        {

        }

        bool fileExists(string path)
        {
            return File.Exists(path);
        }
    }
}

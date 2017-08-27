using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace AMOFGameEngine
{
    /// <summary>
    /// Display on the ModChooser
    /// </summary>
    public class ModBaseInfo
    {
        public readonly string InstallPath;
        public readonly string Name;
        public readonly string Description;
        public readonly string Author;
        public readonly string Thumb;
        public readonly string Movie;

        public ModBaseInfo(string installPath,string name,string description,string author,string thumb,string movie) 
        {
            InstallPath = installPath;
            Name = name;
            Description = description;
            Author = author;
            Thumb = thumb;
            Movie = movie;
        }
    }
}

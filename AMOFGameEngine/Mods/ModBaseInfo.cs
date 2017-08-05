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
        public readonly string Name;
        public readonly string Description;
        public readonly string Author;
        public readonly string Thumb;

        public ModBaseInfo(string name,string description,string author,string thumb) 
        {
            Name = name;
            Description = description;
            Author = author;
            Thumb = thumb;
        }
    }
}
